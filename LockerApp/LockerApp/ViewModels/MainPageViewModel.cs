﻿using System;
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
    using Windows.UI.Xaml.Controls;
    using MvvmDialogs;
    using Prism.Commands;

    class MainPageViewModel :ViewModelBase
    {
        private readonly INavigationService _navService;
        private readonly IFacade _facade;
        private  IDialogService _dialogService;
        private readonly IGpioController _gpioControllerService;
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

        public  MainPageViewModel(INavigationService navigationService, IFacade facade, IDialogService dialogService, IGpioController gpioControllerService) : base(navigationService,facade)
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
            var listenForMessagesThread = new Thread(async()=>await this.ListenForMessages(10000));
            listenForMessagesThread.Start();
           
            

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

               var result= await this._facade.CheckPin(pinToCheck.Code);
                if (result.HasBeenSuccessful)
                {
                    var resultedPin = result.Content;
                    if (resultedPin.UserType == PickerTypeEnum.Courier)
                    {
                      
                            var viewModel = new DeliveryCompanyPageViewModel(this._navService, this._facade, this._dialogService);
                            var dialogResult = await this._dialogService.ShowContentDialogAsync(viewModel);
                            if (dialogResult == ContentDialogResult.Primary)
                            {
                                var currentCompanyChosen = viewModel.CompanySelected;
                                resultedPin.ParcelContactDetails.DeliveryCompanyName = currentCompanyChosen;
                            }


                        
                        var actionResult = await this._facade.AddNewActionForLocker(LockerActionEnum.Delivered, resultedPin);
                        if (!actionResult.HasBeenSuccessful)
                        {
                            SendPinForVerification();
                        }
                        else
                        {
                            this._navService.Navigate(Constants.NavigationPages.PinAcceptedPage,null);
                        }

                    }
                    else if (resultedPin.UserType== PickerTypeEnum.Friend)
                    {
                        var actionResult = await this._facade.AddNewActionForLocker(LockerActionEnum.PickedUp, resultedPin);
                        if (!actionResult.HasBeenSuccessful)
                        {
                            SendPinForVerification();
                        }
                        else
                        {

                            this._navService.Navigate(Constants.NavigationPages.PinAcceptedPage, null);
                        }
                    }
                }
                else
                {
                    await this._dialogService.ShowMessageDialogAsync("This pin is invalid. Please try again or contact the owner in case this is a pickup pin", "Error", new[]
                    {
                        new UICommand { Label = "OK" },

                    });
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
                this._gpioControllerService.CloseLocker();
                this._lockerOpenedCounter = 30;
               
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
                                this._gpioControllerService.CloseLocker();
                            }
                            else
                            {
                                this._gpioControllerService.OpenLocker();
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

