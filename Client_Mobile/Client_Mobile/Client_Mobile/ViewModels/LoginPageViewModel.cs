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
    using Prism.Navigation;
    using Prism.Services;
    using ServiceModels;
    using Xamarin.Essentials;

    public class LoginPageViewModel : ViewModelBase
	{
        #region public properties
        public string Password
        {
            get => this._loginRequest.Password ?? (this.Password = "");
            set => this._loginRequest.Password = value;
        }

        public string Email
        {
            get => this._loginRequest.Email ?? (this.Email = "");
            set => this._loginRequest.Email = value;
        }
        #endregion

        #region public commands
        public DelegateCommand LogInCommand { get; set; }
        public DelegateCommand RegisterCommand { get; set; }

        #endregion

        #region private variables

        private LoginRequest _loginRequest;
        private readonly IFacade _facade;
        private readonly INavigationService _navService;
        private readonly IPageDialogService _dialogService;
        #endregion

        public LoginPageViewModel(INavigationService navigationService, IFacade facadeImplementation, IPageDialogService dialogService)
            : base(navigationService, facadeImplementation, dialogService)
        {
            Title = "Login Page";
            this._loginRequest = new LoginRequest();
            this._navService = navigationService;
            this._dialogService = dialogService;
            this.LogInCommand = new DelegateCommand(async () => await this.Login(this._loginRequest));
            this.RegisterCommand = new DelegateCommand(async () => await this._navService.NavigateAsync(nameof(Views.RegisterPage)));
            this._facade = facadeImplementation;
        }

        public async Task Login(LoginRequest request)
        {
            if (request.Email.Length == 0 || request.Password.Length == 0)
            {
                await this._dialogService.DisplayAlertAsync("Error",
                      "The email or password field is empty. Please fill them to be able to log in.", "OK");
            }
            else
            {
                IsLoading = true;
                var currentUser = await this._facade.LoginUser(Password,Email);
                IsLoading = false;
                if (currentUser.IsSuccessful && currentUser.Content.ProfileName != null)
                {
                    Constants.CurrentLoggedInUser = currentUser.Content;
                    Constants.IotHubConnectionString = Preferences.Get(PreferencesEnum.IoTHubConnectionString,"");
                    Constants.DeviceName = Preferences.Get(PreferencesEnum.IoTHubConnectionString,"");
                    await this.NavigationService.NavigateAsync(nameof(Views.MainPage));
                }
                else
                {

                    await this._dialogService.DisplayAlertAsync("Error",
                        "Email or Password wrong. Please try again", "OK");
                }

            }



        }
    }
}
