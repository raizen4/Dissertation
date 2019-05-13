using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Client_Mobile.ViewModels
{
    using System.Threading.Tasks;
    using Client_Mobile.Enums;
    using Interfaces;
    using Models;
    using Prism.Navigation;
    using Prism.Services;
    using ServiceModels;
    using Xamarin.Essentials;

    public class RegisterPageViewModel : ViewModelBase
    {
        #region private variables

        private string _displayName;
        private string _password;
        private string _phone;
        private string _email;
        private IFacade _facade;
        private IPageDialogService _dialogService;
        #endregion


        #region publicCommands
        public DelegateCommand RegisterCommand { get; set; }
        #endregion

        #region public props
       
        public string Password
        {
            get => this._password;
            set => this._password = value;
        }

        public string DisplayName
        {
            get => this._displayName;
            set => this._displayName = value;
        }

        public string Email
        {
            get => this._email ;
            set => this._email = value;
        }

        public string Phone
        {
            get => this._phone;
            set => this._phone = value;
        }
        #endregion


        public RegisterPageViewModel(INavigationService navigationService, IFacade facade, IPageDialogService dialogService)
            : base(navigationService,facade,dialogService)
        {
            Title = "Register Form";
            this._facade = facade;
            this._dialogService = dialogService;
           
            this.RegisterCommand = new DelegateCommand(async () => await RegisterUser());

        }

        public async Task RegisterUser()
        {

            if ( Password.Length == 0 || Email.Length == 0 || DisplayName.Length == 0 || Phone.Length==0)
            {
                await this._dialogService.DisplayAlertAsync("Error",
                    "All fields are mandatory. Please fill them all before submitting.", "OK");
            }
            else
            {
                IsLoading = true;
                var result = await this._facade.CreateUser(Email,Password,DisplayName,Phone);
                IsLoading = false;
                if (result.HasBeenSuccessful)
                {
                    await this._dialogService.DisplayAlertAsync("Successful", "The registration has been successful", "OK");
                    await NavigationService.NavigateAsync(nameof(Views.LoginPage));
               
                }
                else
                {
                    await this._dialogService.DisplayAlertAsync("Failed", "Something went wrong. Please try again", "OK");
                }
            }

        }


    }
}
