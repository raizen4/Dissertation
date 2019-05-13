using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client_Mobile.ViewModels
{
    using System.Threading;
    using System.Threading.Tasks;
    using Client_Mobile.ServiceModels;
    using Enums;
    using Interfaces;
    using Microsoft.Azure.Devices.Client;
    using Models;
    using Newtonsoft.Json;
    using Plugin.Toasts;
    using Prism.Services;
    using Services;
    using Xamarin.Forms;
    using DependencyService = Xamarin.Forms.DependencyService;

    public class MainPageViewModel : ViewModelBase
    {
        private readonly IFacade _facade;
        private readonly IPageDialogService _dialogService;
        private readonly INavigationService _navService;
        private int _poolingRate = 10000;
        private Thread listenForMessagesThread;


        public string DisplayName => Constants.CurrentLoggedInUser.DisplayName;
        public DelegateCommand LockCommand { get; set; }
        public DelegateCommand UnlockCommand { get; set; }
        public DelegateCommand NavigateToActivityHistory { get; set; }
        public DelegateCommand NavigateToPinGenerator { get; set; }
        public DelegateCommand NavigateToCurrentPins { get; set; }
        public DelegateCommand LogOut{ get; set; }


        public MainPageViewModel(INavigationService navigationService, IFacade facade, IPageDialogService dialogService)
            : base(navigationService,facade,dialogService)
        {
            Title = "Main Page";
            this._dialogService = dialogService;
            this._facade = facade;
            this._navService = navigationService;
            LockCommand=new DelegateCommand(Lock);
            UnlockCommand = new DelegateCommand(Unlock);
            NavigateToActivityHistory=new DelegateCommand(async()=>await this._navService.NavigateAsync(nameof(Views.ActivityHistoryPage)));
            NavigateToPinGenerator = new DelegateCommand(async()=>await SendNewPin());
            NavigateToCurrentPins=new DelegateCommand(async()=>await this._navService.NavigateAsync(nameof(Views.CurrentPinsPage)));
            LogOut = new DelegateCommand(async () => await this.OnBackButtonPressed());
            this.listenForMessagesThread = new Thread(async () => await ListenForMessages(this._poolingRate));
            this.listenForMessagesThread.Start();
          
        }

        /// <inheritdoc />
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            Destroy();
        }

        public override void Destroy()
        {
            base.Destroy();
            try
            {
                this.listenForMessagesThread.Abort();
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
        public async void Lock()
        {
            try
            {
                var result = await this._facade.Lock();
                if (result)
                {
                    await this._dialogService.DisplayAlertAsync("Successful", "Message successfully sent!",
                        "OK");
                }
                else
                {
                    await this._dialogService.DisplayAlertAsync("Failed", "The command has not succeded! Please try again",
                        "OK");
                }

            }
            catch (Exception e)
            {
                await this._dialogService.DisplayAlertAsync("Failed", "The system encountered an error. Make sure there is a connection to the hub and locker adn try again",
                    "OK");
            }

        }

        public async void Unlock()
        {
            try
            {
                var result = await this._facade.Unlock();
                if (result)
                {
                    await this._dialogService.DisplayAlertAsync("Successful", "Message successfully sent!",
                        "OK");
                }
                else
                {
                    await this._dialogService.DisplayAlertAsync("Failed", "The command has not succeded! Please try again",
                        "OK");
                }

            }
            catch (Exception e)
            {
                await this._dialogService.DisplayAlertAsync("Failed", "The system encountered an error. Make sure there is a connection  locker adn try again",
                    "OK");
            }
        }

        public async Task SendNewPin()
        {
            var dialogResult =await this._dialogService.DisplayAlertAsync("Generate Pin",
                "Please choose who should use this pin", "Courier", "A friend");
            NavigationParameters navParams = new NavigationParameters();
            if (dialogResult)
            {
               
                navParams.Add(LockerAccessEnum.Courier.ToString(),true);
                navParams.Add(LockerAccessEnum.Friend.ToString(), false);
                await this._navService.NavigateAsync(nameof(Views.PinPage), navParams);
            }
            else
            {
                navParams.Add(LockerAccessEnum.Courier.ToString(), false);
                navParams.Add(LockerAccessEnum.Friend.ToString(), true);
                await this._navService.NavigateAsync(nameof(Views.PinPage), navParams);
            }
          

        }

        public async Task<bool> OnBackButtonPressed()
        {
            var dialogResult = await this._dialogService.DisplayAlertAsync("Warning",
                "You will now be logged out. Do you agree", "OK", "Cancel");
            if (dialogResult)
            {
                Constants.CurrentLoggedInUser = null;
                Constants.Token = null;
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task ListenForMessages(int poolingRate)
        {
            while (true)
            {
                var newMesasgeReceived = await this._facade.GetPendingMessagesFromHub();
                if (newMesasgeReceived != null &&
                    newMesasgeReceived.TargetedDeviceId == Constants.CurrentLoggedInUser.DeviceId)
                {
                    if (newMesasgeReceived.Action == LockerActionEnum.UserAppClosed)
                    {
                        var notificator = DependencyService.Get<IToastNotificator>();

                        var options = new NotificationOptions()
                        {
                            Title = "Closed",
                            Description = "Locker successfully closed"
                        };

                     await notificator.Notify(options);
                    }
                    else if (newMesasgeReceived.Action == LockerActionEnum.UserAppOpened)
                    {
                        var notificator = DependencyService.Get<IToastNotificator>();

                        var options = new NotificationOptions()
                        {
                            Title = "Opened",
                            Description = "Locker will be opened for 30 seconds after which it will automatically close."
                        };

                       await notificator.Notify(options);
                    }
                }

                Thread.Sleep(poolingRate);
            }
        }

    }
}
