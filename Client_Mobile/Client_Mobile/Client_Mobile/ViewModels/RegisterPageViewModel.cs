using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Client_Mobile.ViewModels
{
    using System.Threading.Tasks;
    using Interfaces;
    using Models;
    using Prism.Navigation;
    using Prism.Services;
    using ServiceModels;

    public class RegisterPageViewModel : ViewModelBase
    {
        #region private variables

        private string _profileName;
        private string _password;
        private string _lockerId;
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

        public string DeviceProfileName
        {
            get => this._profileName;
            set => this._profileName = value;
        }

        public string Email
        {
            get => this._email ;
            set => this._email = value;
        }

        public string LockerId
        {
            get => this._lockerId;
            set => this._lockerId = value;
        }
        #endregion


        public RegisterPageViewModel(INavigationService navigationService, IFacade facade, IPageDialogService dialogService)
            : base(navigationService)
        {
            Title = "Register Form";
            this._facade = facade;
            this._dialogService = dialogService;
           
            this.RegisterCommand = new DelegateCommand(async () => await RegisterUser());

        }

        public async Task RegisterUser()
        {
            if ( Password.Length == 0 || Email.Length == 0 || DeviceProfileName.Length == 0 || LockerId.Length==0)
            {
                await this._dialogService.DisplayAlertAsync("Error",
                    "All fields are mandatory. Please fill them all before submitting.", "OK");
            }
            else
            {
                
                var result = await this._facade.CreateUser(Email,Password,DeviceProfileName,LockerId);
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
