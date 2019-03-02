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
    using Enums;
    using Interfaces;
    using Microsoft.Azure.Devices.Client;
    using Models;
    using Prism.Services;
    using Services;
    using Xamarin.Forms;

    public class MainPageViewModel : ViewModelBase
    {
        private readonly IFacade _facade;
        private readonly IPageDialogService _dialogService;
        private readonly INavigationService _navService;
        private int _poolingRate = 10000;
        public DelegateCommand LockCommand { get; set; }
        public DelegateCommand UnlockCommand { get; set; }
        public DelegateCommand NavigateToActivityHistory { get; set; }
        public DelegateCommand NavigateToPinGenerator { get; set; }
        public DelegateCommand SendNewPinCommand { get; set; }

     
        public MainPageViewModel(INavigationService navigationService, IFacade facade, IPageDialogService dialogService)
            : base(navigationService)
        {
            Title = "Main Page";
            this._dialogService = dialogService;
            this._facade = facade;
            this._navService = navigationService;
            LockCommand=new DelegateCommand(Lock);
            UnlockCommand = new DelegateCommand(Unlock);
            NavigateToActivityHistory=new DelegateCommand(async()=>await this._navService.NavigateAsync(nameof(Views.ActivityHistoryPage)));
            NavigateToPinGenerator = new DelegateCommand(SendNewPin);
            this.ListenForMessages(this._poolingRate);
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
                await this._dialogService.DisplayAlertAsync("Failed", "The system encountered an error. Make sure there is a connection  locker adn try again",
                    "OK");
            }
        }

        public async void SendNewPin()
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

            navParams.Add(LockerAccessEnum.Courier.ToString(), false);
            navParams.Add(LockerAccessEnum.Friend.ToString(), true);
            await this._navService.NavigateAsync(nameof(Views.PinPage),navParams);

        }

        private async void ListenForMessages(int poolingRate)
        {
         
            while (true)
            {
                var newMesasgeReceived = await this._facade.GetPendingMessagesFromHub();
                if (newMesasgeReceived != null)
                {
                    string stringifiedAction;
                    int normalizedAction = 0;

                    try
                    {
                        newMesasgeReceived.Properties.TryGetValue("Action", out stringifiedAction);
                        normalizedAction = int.Parse(stringifiedAction);
                    }
                    catch (Exception e)
                    {
                        //silently fail
                    }

                    if (normalizedAction == 0)
                    {
                        //
                    }
                    else if (normalizedAction == (int)LockerActionEnum.Opened)
                    {
                        await this._dialogService.DisplayAlertAsync("Successful", " Locker has been opened!",
                            "OK");
                    }
                    else if (normalizedAction == (int)LockerActionEnum.Closed)
                    {
                        await this._dialogService.DisplayAlertAsync("Successful", " Locker has been closed!",
                            "OK");
                    }
                    else if (normalizedAction == (int)LockerActionEnum.NewPinGenerated)
                    {
                        await this._dialogService.DisplayAlertAsync("Successful", " New pin has been added to the locker.",
                            "OK");
                    }

                    Thread.Sleep(poolingRate);
                }
            }
          
        

        }
    }
}
