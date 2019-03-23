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
    class MainPageViewModel : ViewModelBase
    {
        private readonly INavigationService navService;
        private readonly IFacade facade;
        private string pin;
        private int lockerOpenedCounter;
        private static DeviceClient _deviceClient;
        private DispatcherTimer timer;


        public string Pin { get; set; }
        public MainPageViewModel(INavigationService navigationService, IFacade facade) : base(navigationService,facade)
        {
            this.navService = navigationService;
            this.facade = facade;
            this.IsLoading = false;
            this.lockerOpenedCounter = 30;
        }


        public async void SendPinForVerification(string pinInserted)
        {
            if (pinInserted.Length < 6)
            {
                var showDialogResult=await this.DisplayDialog("Error", "The pin must be 6 digits long", 1, "OK",null);
                return;
            }
            var pinToCheck = new Pin()
            {
                Code = pinInserted
            };
            try
            {

               var result= await this.facade.CheckPin(pinToCheck);
                if (result.IsSuccessful)
                {
                    var resultedPin = result.Content;
                    if (resultedPin.PickerType == PickerTypeEnum.Courier)
                    {
                        var actionResult = await this.facade.AddNewActionForLocker(LockerActionRequestsEnum.Delivered, resultedPin);
                        if (!actionResult.IsSuccessful)
                        {
                            this.SendPinForVerification(pinInserted);
                        }
                        else
                        {
                            this.navService.Navigate(Constants.NavigationPages.PinAcceptedPage,null);
                        }

                    }
                    else if (resultedPin.PickerType== PickerTypeEnum.Friend)
                    {
                        var actionResult = await this.facade.AddNewActionForLocker(LockerActionRequestsEnum.PickedUp, resultedPin);
                        if (!actionResult.IsSuccessful)
                        {
                            this.SendPinForVerification(pinInserted);
                        }
                        else
                        {
                            this.navService.Navigate(Constants.NavigationPages.PinAcceptedPage, null);
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
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += TimerTick;
        }
        void StartTimer()
        {
            timer.Start();
        }
        void TimerTick(object sender, object e)
        {
            this.lockerOpenedCounter--;
           
            if (lockerOpenedCounter == 0)
            {
                timer.Stop();
                this.CloseLocker();
                this.lockerOpenedCounter = 30;
               
            }


        }

        private async void ListenForMessages(int poolingRate)
        {
            var shouldClose = false;
            var shouldOpen = false;
            while (true)
            {
                var newMesasgeReceived = await this.facade.GetPendingMessagesFromHub();
                if (newMesasgeReceived != null)
                {
                    var stringMessage = newMesasgeReceived.ToString();
                    var deserializedMessage = JsonConvert.DeserializeObject<LockerMessage>(stringMessage);
                    if (deserializedMessage.ActionRequest == LockerActionRequestsEnum.UserAppClose ||
                        deserializedMessage.ActionRequest == LockerActionRequestsEnum.UserAppOpen)
                    {
                        if(deserializedMessage.ActionRequest == LockerActionRequestsEnum.UserAppOpen)
                        {
                            deserializedMessage.ActionResult = LockerActionRequestsEnum.UserAppOpened;
                        }
                        else
                        {
                            deserializedMessage.ActionResult = LockerActionRequestsEnum.UserAppClosed;
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
                                this.CloseLocker();
                            }
                            else
                            {
                                this.OpenLocker();
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

