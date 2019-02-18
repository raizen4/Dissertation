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
        private readonly IFacade facade;
        private readonly IPageDialogService dialogService;    
        private int poolingRate = 10000;
        public DelegateCommand LockCommand { get; set; }
        public DelegateCommand UnlockCommand { get; set; }
        public DelegateCommand SendNewPinCommand { get; set; }

     
        public MainPageViewModel(INavigationService navigationService, IFacade facade, IPageDialogService dialogService)
            : base(navigationService)
        {
            Title = "Main Page";
            this.dialogService = dialogService;
            this.facade = facade;
           
            LockCommand=new DelegateCommand(Lock);
            UnlockCommand = new DelegateCommand(Unlock);
            SendNewPinCommand = new DelegateCommand(SendNewPin);
            this.ListenForMessages(this.poolingRate);
        }


        public async void Lock()
        {
            try
            {
                var result = await this.facade.Lock();
                if (result)
                {
                    await this.dialogService.DisplayAlertAsync("Successful", "Message successfully sent!",
                        "OK");
                }
                else
                {
                    await this.dialogService.DisplayAlertAsync("Failed", "The command has not succeded! Please try again",
                        "OK");
                }

            }
            catch (Exception e)
            {
                await this.dialogService.DisplayAlertAsync("Failed", "The system encountered an error. Make sure there is a connection to the hub and locker adn try again",
                    "OK");
            }

        }

        public async void Unlock()
        {
            try
            {
                var result = await this.facade.Lock();
                if (result)
                {
                    await this.dialogService.DisplayAlertAsync("Successful", "Message successfully sent!",
                        "OK");
                }
                else
                {
                    await this.dialogService.DisplayAlertAsync("Failed", "The command has not succeded! Please try again",
                        "OK");
                }

            }
            catch (Exception e)
            {
                await this.dialogService.DisplayAlertAsync("Failed", "The system encountered an error. Make sure there is a connection  locker adn try again",
                    "OK");
            }
        }

        public async void SendNewPin()
        {
            try
            {
               Pin newPin=new Pin(){Code = "12331", IssuerId = "21434123", Ttl = "24"};

                var result = await this.facade.SendPinToLocker(newPin);
                if (result)
                {
                    await this.dialogService.DisplayAlertAsync("Successful", "Message successfully sent!",
                        "OK");
                }
                else
                {
                    await this.dialogService.DisplayAlertAsync("Failed", "The command has not succeded! Please try again",
                        "OK");
                }

            }
            catch (Exception e)
            {
                await this.dialogService.DisplayAlertAsync("Failed", "The system encountered an error. Make sure there is a connection to the hub and locker adn try again",
                    "OK");
            }
        }

        public async void ListenForMessages(int poolingRate)
        {
            var newMesasgeReceived = await this.facade.GetPendingMessagesFromHub();
            while (true)
            {
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
                        await this.dialogService.DisplayAlertAsync("Successful", " Locker has been opened!",
                            "OK");
                    }
                    else if (normalizedAction == (int)LockerActionEnum.Closed)
                    {
                        await this.dialogService.DisplayAlertAsync("Successful", " Locker has been closed!",
                            "OK");
                    }
                    else if (normalizedAction == (int)LockerActionEnum.NewPinGenerated)
                    {
                        await this.dialogService.DisplayAlertAsync("Successful", " New pin has been added to the locker.",
                            "OK");
                    }

                    Thread.Sleep(poolingRate);
                }
            }
          
        

        }
    }
}
