namespace XamarinLockerApp.ViewModels
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Enums;
    using Interfaces;
    using Microsoft.Azure.Devices.Client;
    using Models;
    using Newtonsoft.Json;
    using Prism.Commands;
    using Prism.Navigation;
    using Prism.Services;
    using ServiceModels;
    using Views;
    using Xamarin.Forms;

    class MainPageViewModel :ViewModelBase
    {
        private readonly INavigationService _navService;
        private readonly IFacade _facade;
        private  IPageDialogService _dialogService;
        private string _pin;
        private int _lockerOpenedCounter;
        private static DeviceClient _deviceClient;
        private Timer _timer;
        private bool _resetTimer;
        private Thread listenForMessagesThread;
        private Thread listenForChangesInPowerStatus;
        private IIoTHub hub;
        public DelegateCommand SendPinForCheckingCommand { get; set; }
        public string Pin
        {
            get => this._pin;
            set => this._pin = value;
        }

        public  MainPageViewModel(INavigationService navigationService, IFacade facade, IPageDialogService dialogService, IIoTHub hub) : base(navigationService,facade)
        {
            this._dialogService = dialogService;
            this._navService = navigationService;
            this._facade = facade;
            Pin = "";
            this.hub = hub;
            this._resetTimer = false;
            IsLoading = false;
            this._lockerOpenedCounter = 30;
            SendPinForCheckingCommand=new DelegateCommand(SendPinForVerification);
       


            this.listenForMessagesThread = new Thread(async()=>await ListenForMessages(5000));
            this.listenForMessagesThread.Start();
            this.listenForChangesInPowerStatus = new Thread(async () => await GetPowerStatus(10000));
            this.listenForChangesInPowerStatus.Start();



        }

        public async Task GetPowerStatus(int poolingRateMiliseconds)
        {
            try
            {
                while (true)
                {
                    var currentPowerStatus = await this._facade.GetLockerPowerStatus();
                    if (Constants.UserLocker.PowerStatus == null)
                    {
                        if (currentPowerStatus.Content == PowerTypeEnum.MainPower)
                        {
                            Constants.UserLocker.PowerStatus = PowerStatusEnum.MainPower;
                        }
                        else
                        {
                            Constants.UserLocker.PowerStatus = PowerStatusEnum.BackupPower;
                            await this._facade.SendPowerStatusChangedNotification(PowerTypeEnum.BackupPower);
                        }
                    }


                    else
                    {
                        if (currentPowerStatus.Content == PowerTypeEnum.BackupPower &&
                            Constants.UserLocker.PowerStatus == PowerStatusEnum.MainPower)
                        {
                            Constants.UserLocker.PowerStatus = PowerStatusEnum.BackupPower;
                            await this._facade.SendPowerStatusChangedNotification(PowerTypeEnum.BackupPower);
                        }
                        else if (currentPowerStatus.Content == PowerTypeEnum.MainPower &&
                                 Constants.UserLocker.PowerStatus == PowerStatusEnum.BackupPower)
                        {
                            Constants.UserLocker.PowerStatus = PowerStatusEnum.MainPower;

                            await this._facade.SendPowerStatusChangedNotification(PowerTypeEnum.MainPower);
                        }
                    }
                 
                  

                    Thread.Sleep(poolingRateMiliseconds);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine();
            }
        }
      
        public async void SendPinForVerification()
        {
            if (Pin.Length < 5)
            {
                await this._dialogService.DisplayActionSheetAsync("Error",
                    "The pin must be at least 5 digits. Please try again", "OK"
                );
                Pin = "";
                return;
            }

            var pinToCheck = new Pin()
            {
                Code = Pin
            };
            if (Pin == "OPTION")
            {
               await this._navService.NavigateAsync(nameof(Views.LockerInfoPage), null);
                return;
            }
            try
            {
                IsLoading = true;
               var result= await this._facade.CheckPin(pinToCheck.Code);
                IsLoading = false;

                if (result.HasBeenSuccessful)
                {
                    var resultedPin = result.Content;
                    if (resultedPin.UserType == PickerTypeEnum.Courier)
                    {

                   
                        await this._navService.NavigateAsync(nameof(DeliveryCompanyPage));
                        

                    }
                    else if (resultedPin.UserType== PickerTypeEnum.Friend)
                    {
                        IsLoading = true;

                        await this.RunFriendCase(resultedPin);
                        IsLoading = false;

                    }
                }
                else
                {
                    await this._dialogService.DisplayActionSheetAsync("Error",
                        "This pin is invalid. Please try again or contact the owner in case this is a pickup pin", "OK"
                    );    
                }

            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <inheritdoc />
        public override void Destroy()
        {
            base.Destroy();
            try
            {
                this.listenForMessagesThread.Abort();
                this.listenForChangesInPowerStatus.Abort();
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
       
        }

        private async Task RunCourierCase(Pin resultedPin)
        {
            IsLoading = true;

            var actionResult = await this._facade.AddNewActionForLocker(LockerActionEnum.Delivered, resultedPin);
            IsLoading = false;

            if (!actionResult.HasBeenSuccessful)
            {
                SendPinForVerification();
            }
            else
            {
                await this._navService.NavigateAsync(nameof(Views.FinishOpenLockerPage), null);
            }
        }

        private async Task RunFriendCase(Pin resultedPin)
        {
            IsLoading = true;

            var actionResult = await this._facade.AddNewActionForLocker(LockerActionEnum.PickedUp, resultedPin);
            IsLoading = false;

            if (!actionResult.HasBeenSuccessful)
            {
                SendPinForVerification();
            }
            else
            {

                await this._navService.NavigateAsync(nameof(Views.FinishOpenLockerPage), null);
            }
        }
        /// <inheritdoc />
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            try
            {
                var courierCompany = parameters.GetValue<string>("COMPANY");
                if (courierCompany != null)
                {
                    var pinToCheck = new Pin()
                    {
                        Code = Pin,
                        ParcelContactDetails = new ContactDetails()
                        {
                            DeliveryCompanyName = courierCompany
                        }
                    };
                    await this.RunCourierCase(pinToCheck);
                }

            }
            catch (Exception e)
            {
                //silently fail
            }
        }

        /// <inheritdoc />
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            Destroy();
        }

        private async Task ListenForMessages(int poolingRate)
        {
            
                
            var shouldClose = false;
            while (true)
            {
                var newMesasgeReceived = await this._facade.GetPendingMessagesFromHub();
                if (newMesasgeReceived != null && newMesasgeReceived.TargetedDeviceId==Constants.UserLocker.DeviceId)
                {
                   
                  
                    if (newMesasgeReceived.Action == LockerActionEnum.UserAppClose
                        ||
                        newMesasgeReceived.Action == LockerActionEnum.UserAppOpen)
                    {
                        if(newMesasgeReceived.Action == LockerActionEnum.UserAppOpen)
                        {
                            newMesasgeReceived.Action = LockerActionEnum.UserAppOpened;
                            newMesasgeReceived.TargetedDeviceId = newMesasgeReceived.SenderDeviceId;
                            newMesasgeReceived.SenderDeviceId = Constants.UserLocker.DeviceId;


                        }
                        else
                        {
                            newMesasgeReceived.Action = LockerActionEnum.UserAppClosed;
                            newMesasgeReceived.TargetedDeviceId = newMesasgeReceived.SenderDeviceId;
                            newMesasgeReceived.SenderDeviceId = Constants.UserLocker.DeviceId;
                            shouldClose = true;
                        }

                       
                        await this.hub.SendMessageToLocker(newMesasgeReceived);
                        try
                        {
                           
                            if (shouldClose)
                            {

                                var actionResult = await this._facade.AddNewActionForLocker(LockerActionEnum.UserAppClosed, new Pin());

                                if (actionResult.HasBeenSuccessful)
                                {
                                    Device.BeginInvokeOnMainThread(async () => await this._navService.NavigateAsync(nameof(FinishCloseLockerPage)));

                                 
                                }
                             
                            }
                            else
                            {
                                var actionResult = await this._facade.AddNewActionForLocker(LockerActionEnum.UserAppOpened, new Pin());

                                if (actionResult.HasBeenSuccessful)
                                {
                                    Device.BeginInvokeOnMainThread(async () => await this._navService.NavigateAsync(nameof(FinishOpenLockerPage), null));
                                  
                                }
                            

                              
                                
                            }

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);                           
                        }

                    }
                    }

                    Thread.Sleep(poolingRate);
                }
            }

        }
    }

