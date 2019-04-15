using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LockerAppXamarin.Enums;
using LockerAppXamarin.Interfaces;
using LockerAppXamarin.Models;
using LockerAppXamarin.ServiceModels;

namespace LockerAppXamarin.ViewModels
{
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
        private readonly IGpioController _gpioControllerService;
        private string _pin;
        private int _lockerOpenedCounter;
        private static DeviceClient _deviceClient;
        private Timer _timer;
        private bool _resetTimer;

        public DelegateCommand SendPinForCheckingCommand { get; set; }
        public string Pin
        {
            get => this._pin;
            set => this._pin = value;
        }

        public  MainPageViewModel(INavigationService navigationService, IFacade facade, IPageDialogService dialogService, IGpioController gpioControllerService) : base(navigationService,facade)
        {
            this._gpioControllerService = gpioControllerService;
            this._dialogService = dialogService;
            this._navService = navigationService;
            this._facade = facade;
            Pin = "";
            this._resetTimer = false;
            IsLoading = false;
            this._lockerOpenedCounter = 30;
            SendPinForCheckingCommand=new DelegateCommand(SendPinForVerification);
            var listenForMessagesThread = new Thread(async()=>await ListenForMessages(10000));
            listenForMessagesThread.Start();
           
            

        }


        public async void SendPinForVerification()
        {
            if (Pin.Length < 6)
            {
                await this._dialogService.DisplayActionSheetAsync("Error",
                    "The pin must be at least 6 digits. Please try again", "OK"
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

               var result= await this._facade.CheckPin(pinToCheck.Code);
                if (result.HasBeenSuccessful)
                {
                    var resultedPin = result.Content;
                    if (resultedPin.UserType == PickerTypeEnum.Courier)
                    {

                   
                        await this._navService.NavigateAsync(nameof(Views.DeliveryCompanyPage));
                        

                    }
                    else if (resultedPin.UserType== PickerTypeEnum.Friend)
                    {
                       await this.RunFriendCase(resultedPin);
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

        private async Task RunCourierCase(Pin resultedPin)
        {

            var actionResult = await this._facade.AddNewActionForLocker(LockerActionEnum.Delivered, resultedPin);
            if (!actionResult.HasBeenSuccessful)
            {
                SendPinForVerification();
            }
            else
            {
                await this._navService.NavigateAsync(nameof(Views.FinishPage), null);
            }
        }

        private async Task RunFriendCase(Pin resultedPin)
        {
            var actionResult = await this._facade.AddNewActionForLocker(LockerActionEnum.PickedUp, resultedPin);
            if (!actionResult.HasBeenSuccessful)
            {
                SendPinForVerification();
            }
            else
            {

                await this._navService.NavigateAsync(nameof(Views.FinishPage), null);
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

        private async Task ListenForMessages(int poolingRate)
        {
            
                
            var shouldClose = false;
            while (true)
            {
                var newMesasgeReceived = await this._facade.GetPendingMessagesFromHub();
                if (newMesasgeReceived != null)
                {
                    var stringMessage = newMesasgeReceived.ToString();
                    var deserializedMessage = JsonConvert.DeserializeObject<LockerMessage>(stringMessage);
                    if (deserializedMessage.Action == LockerActionEnum.UserAppClose
                        ||
                        deserializedMessage.Action == LockerActionEnum.UserAppOpen)
                    {
                        if(deserializedMessage.Action == LockerActionEnum.UserAppOpen)
                        {
                            deserializedMessage.ActionResult = LockerActionEnum.UserAppOpened;
                            deserializedMessage.TargetedDeviceId = deserializedMessage.SenderDeviceId;
                            deserializedMessage.SenderDeviceId = Constants.UserLocker.DeviceId;


                        }
                        else
                        {
                            deserializedMessage.ActionResult = LockerActionEnum.UserAppClosed;
                            deserializedMessage.TargetedDeviceId = deserializedMessage.SenderDeviceId;
                            deserializedMessage.SenderDeviceId = Constants.UserLocker.DeviceId;
                            shouldClose = true;
                        }
                        
                        deserializedMessage.HasBeenSuccessful = true;
                        var messageString = JsonConvert.SerializeObject(deserializedMessage);
                        byte[] messageBytes = Encoding.UTF8.GetBytes(messageString);
                        var message = new Message(messageBytes);
                        message.Properties.Add("IotHubEndpoint", IotEndpointsEnum.D2DEndpoint);
                        try
                        {
                            await _deviceClient.SendEventAsync(message);
                            if (shouldClose)
                            {
                                await this._navService.NavigateAsync(nameof(FinishCloseLockerPage));
                            }
                            else
                            {
                                var navParams = new NavigationParameters();
                                navParams.Add("");
                                await this._navService.NavigateAsync(nameof(FinishPage));

                              
                                
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

