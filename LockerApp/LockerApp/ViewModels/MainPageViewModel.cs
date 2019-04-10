using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LockerApp.Enums;
using LockerApp.Interfaces;
using LockerApp.Models;
using LockerApp.ServiceModels;
using LockerApp.Services;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using Prism.Windows.Navigation;
using Windows.UI.Xaml;

namespace LockerApp.ViewModels
{
    using Windows.UI.Popups;
    using MvvmDialogs;
    using Prism.Commands;

    class MainPageViewModel :ViewModelBase
    {
        private readonly INavigationService _navService;
        private readonly IFacade _facade;
        private  IDialogService _dialogService;
        private readonly IGpioController _gpioController;
        private string _pin;
        private int _lockerOpenedCounter;
        private static DeviceClient _deviceClient;
        private DispatcherTimer _timer;
        private bool _resetTimer;

        public DelegateCommand SendPinForCheckingCommand { get; set; }
        public string Pin
        {
            get => this._pin;
            set => this._pin = value;
        }

        public MainPageViewModel(INavigationService navigationService, IFacade facade, IDialogService dialogService, IGpioController gpioController) : base(navigationService,facade)
        {
            this._gpioController = gpioController;
            this._dialogService = dialogService;
            this._navService = navigationService;
            this._facade = facade;
            Pin = "";
            this._resetTimer = false;
            IsLoading = false;
            this._lockerOpenedCounter = 30;
            SendPinForCheckingCommand=new DelegateCommand(SendPinForVerification);
            ListenForMessages(10000);

        }


        public async void SendPinForVerification()
        {
            if (Pin.Length < 6)
            {
                await this._dialogService.ShowMessageDialogAsync("The pin must be at least 6 digits. Please try again","Error", new[]
                {
                    new UICommand { Label = "OK" },
                    
                });
                Pin = "";
                return;
            }
            var pinToCheck = new Pin()
            {
                Code = Pin
            };
            if (Pin == "OPTION")
            {
                this._navService.Navigate(Constants.NavigationPages.InfoPage, null);
                return;
            }
            try
            {

               var result= await this._facade.CheckPin(pinToCheck);
                if (result.IsSuccessful)
                {
                    var resultedPin = result.Content;
                    if (resultedPin.PickerType == PickerTypeEnum.Courier)
                    {
                        var actionResult = await this._facade.AddNewActionForLocker(LockerActionRequestsEnum.Delivered, resultedPin);
                        if (!actionResult.IsSuccessful)
                        {
                            SendPinForVerification();
                        }
                        else
                        {
                            this._navService.Navigate(Constants.NavigationPages.PinAcceptedPage,null);
                        }

                    }
                    else if (resultedPin.PickerType== PickerTypeEnum.Friend)
                    {
                        var actionResult = await this._facade.AddNewActionForLocker(LockerActionRequestsEnum.PickedUp, resultedPin);
                        if (!actionResult.IsSuccessful)
                        {
                            SendPinForVerification();
                        }
                        else
                        {

                            this._navService.Navigate(Constants.NavigationPages.PinAcceptedPage, null);
                        }
                    }
                }

            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        void InitializeTimer()
        {
            this._timer = new DispatcherTimer();
            this._timer.Interval = new TimeSpan(0, 0, 1);
            this._timer.Tick += TimerTick;
            StartTimer();
        }
        void StartTimer()
        {
            this._timer.Start();
        }
        void TimerTick(object sender, object e)
        {
            this._lockerOpenedCounter--;

            if (this._lockerOpenedCounter == 0 || this._resetTimer) 
            {
                this._timer.Stop();
                this._gpioController.CloseLocker();
                this._lockerOpenedCounter = 30;
               
            }


        }

        private async void ListenForMessages(int poolingRate)
        {
            var shouldClose = false;
            while (true)
            {
                var newMesasgeReceived = await this._facade.GetPendingMessagesFromHub();
                if (newMesasgeReceived != null)
                {
                    var stringMessage = newMesasgeReceived.ToString();
                    var deserializedMessage = JsonConvert.DeserializeObject<LockerMessage>(stringMessage);
                    if (deserializedMessage.ActionRequest == LockerActionRequestsEnum.UserAppClose
                        ||
                        deserializedMessage.ActionRequest == LockerActionRequestsEnum.UserAppOpen)
                    {
                        if(deserializedMessage.ActionRequest == LockerActionRequestsEnum.UserAppOpen)
                        {
                            deserializedMessage.ActionResult = LockerActionRequestsEnum.UserAppOpened;
                            deserializedMessage.TargetedDeviceId = deserializedMessage.SenderDeviceId;
                            deserializedMessage.SenderDeviceId = Constants.UserLocker.DeviceId;


                        }
                        else
                        {
                            deserializedMessage.ActionResult = LockerActionRequestsEnum.UserAppClosed;
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
                                this._gpioController.CloseLocker();
                            }
                            else
                            {
                                this._gpioController.OpenLocker();
                                InitializeTimer();
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

