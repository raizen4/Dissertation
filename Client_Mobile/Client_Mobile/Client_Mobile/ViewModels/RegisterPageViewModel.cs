using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Client_Mobile.ViewModels
{
    using System.Threading.Tasks;
    using Interfaces;
    using Prism.Navigation;
    using Prism.Services;
    using ServiceModels;

    public class RegisterPageViewModel : ViewModelBase
    {
        #region private variables
        private RegisterRequest _registerRequest;
        private IFacade _facade;
        private IPageDialogService _dialogService;
        #endregion


        #region publicCommands
        public DelegateCommand RegisterCommand { get; set; }
        #endregion

        #region public props
       
        public string Password
        {
            get => this._registerRequest.Password ?? (this._registerRequest.Password = "");
            set => this._registerRequest.Password = value;
        }

        public string ProfileId
        {
            get => this._registerRequest.ProfileId ?? (this._registerRequest.ProfileId = "");
            set => this._registerRequest.ProfileId = value;
        }

        public string Email
        {
            get => this._registerRequest.Email ?? (this._registerRequest.Email = "");
            set => this._registerRequest.Email = value;
        }
        #endregion


        public RegisterPageViewModel(INavigationService navigationService, IFacade facade, IPageDialogService dialogService)
            : base(navigationService)
        {
            Title = "Register Form";
            this._facade = facade;
            this._dialogService = dialogService;
            this._registerRequest = new RegisterRequest();
            this.RegisterCommand = new DelegateCommand(async () => await RegisterUser());

        }

        public async Task RegisterUser()
        {
            if ( Password.Length == 0 || Email.Length == 0 || ProfileId.Length == 0)
            {
                await this._dialogService.DisplayAlertAsync("Error",
                    "All fields are mandatory. Please fill them all before submitting.", "OK");
            }
            else
            {
                var registerForm = new RegisterRequest();
              
                registerForm.Password = Password.Trim();
                registerForm.ProfileId = ProfileId.Trim();
                registerForm.Email = Email.Trim();

                var result = await this._facade.CreateUser(Email,Password,ProfileId);
                if (result.Error == null && result.IsSuccessful)
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
