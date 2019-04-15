﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LockerAppXamarin.Helpers;
using LockerAppXamarin.Enums;
using LockerAppXamarin.Interfaces;
using Prism.Commands;

namespace LockerAppXamarin.ViewModels
{
    using System.ComponentModel;
    using Enums;
    using Helpers;
    using Interfaces;
    using Prism.Navigation;
    using Prism.Services;
    using Views;
    using Xamarin.Essentials;

    public class StartPageViewModel :ViewModelBase
    {
        private readonly IFacade _facade;
        private readonly INavigationService _navService;
        private readonly IPageDialogService _dialogService;
        private string _password;
        private string _username;
      //  private IDialogService dialogService;
   


        public DelegateCommand LoginCommand { get; set; }
        public string Password
        {
            get => this._password;
            set
            {
                this._password = value;
                RaisePropertyChanged();
            }
        }

        public  string Username
        {
            get => this._username;
            set
            {
                this._username = value; 
                RaisePropertyChanged();
            }
        }

        public StartPageViewModel(INavigationService navigationService, IFacade facade, IPageDialogService dialogService) : base(navigationService,facade)
           {
           this._dialogService = dialogService;
            this._facade = facade;
            this._navService = navigationService;
         
            LoginCommand = new DelegateCommand(async()=>await LoginUser(Password,Username));
          
            if (SecureStorage.GetAsync("PASSWORD") != null)
            {
                //check first if we already have the credetials stored, 
                //if yes make sure you change the successfully logged in message

                 Password =  SecureStorage.GetAsync("PASSWORD").GetAwaiter().GetResult(); 
                 Username = SecureStorage.GetAsync("USERNAME").GetAwaiter().GetResult();

                LoginUser(Password, Username);

            }
          

        }

       

         
        private async Task<bool> InitializeLocker(string pass, string user)
        {
           var lockerId ="Locker"+PinGenerator.GeneratePin();
           var lockerInit = await this._facade.AddNewLocker(lockerId);
            if (lockerInit.HasBeenSuccessful)
            {
                Constants.UserLocker = lockerInit.Content;
                return true;
            }
            await this._dialogService.DisplayActionSheetAsync("Error",
                "Couldn't register locker. Please try again", "OK"
            );
          
            return false;

        }

        public async Task LoginUser(string pass, string user)
        {
           

                if (pass.Length == 0 || user.Length == 0)
            {
                await this._dialogService.DisplayActionSheetAsync("Error",
                    "All fields are mandatory", "OK"
                );
             
                return;
            }


            var loginResult = await this._facade.LoginUser(pass, user);
            if (loginResult.HasBeenSuccessful)
            {
                Constants.Token = loginResult.Content.Token;
                Constants.UserLocker = loginResult.Content;
             
                if (await SecureStorage.GetAsync("PASSWORD")!=null)
                {

                    await this._dialogService.DisplayActionSheetAsync("Successful",
                        "Auto Logging has been successful", "OK"
                    );

                 
                  
                   await this._navService.NavigateAsync(nameof(MainPage), null);
                }
                else
                {               
                        var result = await InitializeLocker(pass, user);
                        if (!result)
                        {
                            return;
                        }
                    try
                    {
                        await SecureStorage.SetAsync("PASSWORD", pass);
                        await SecureStorage.SetAsync("USERNAME", user);
                    }
                    catch (Exception ex)
                    {
                        // Possible that device doesn't support secure storage on device.
                    }
                    await this._dialogService.DisplayActionSheetAsync("Linked",
                        "The account has been linked to this locker.", "OK"
                    );
                 
                    
                   await this._navService.NavigateAsync(nameof(MainPage), null);
                }
               
            }
            else
            {
                await this._dialogService.DisplayActionSheetAsync("Unsuccessful login",
                    "Login failed. Please check the credentials and try again", "OK"
                );
             
               

            }

        }

       
       
    }
}