using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client_Mobile.ViewModels
{
    using Enums;
    using Interfaces;
    using Models;
    using Prism.Services;

    public class MainPageViewModel : ViewModelBase
    {
        private readonly IFacade facade;
        private readonly IPageDialogService dialogService;
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
        }


        public async void Lock()
        {
            try
            {
                var result = await this.facade.Lock(LockerActionEnum.Close);
                if (result)
                {
                    await this.dialogService.DisplayAlertAsync("Successful", "The locker has been locked!",
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
                var result = await this.facade.Lock(LockerActionEnum.Open);
                if (result)
                {
                    await this.dialogService.DisplayAlertAsync("Successful", "The locker has been locked!",
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

        public async void SendNewPin()
        {
            try
            {
               Pin newPin=new Pin(){};

                var result = await this.facade.Lock(LockerActionEnum.Close);
                if (result)
                {
                    await this.dialogService.DisplayAlertAsync("Successful", "The locker has been locked!",
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
    }
}
