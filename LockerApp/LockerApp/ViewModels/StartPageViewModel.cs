using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client_Mobile.Helpers;
using LockerApp.Enums;
using LockerApp.Interfaces;
using Prism.Commands;
using Prism.Windows.Navigation;
using Windows.Security.Credentials;
using Windows.Storage;

namespace LockerApp.ViewModels
{
    class StartPageViewModel : ViewModelBase
    {
        private readonly IFacade facade;
        private readonly INavigationService navService;
        private PasswordVault vault;
        public DelegateCommand LoginCommand { get; set; }
        private string password;
        private string username;
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public string Password {
            get => this.password;
            set => this.password = value;
        }
        public  string Username {
            get => this.username;
            set => this.username = value;
        }

           public StartPageViewModel(INavigationService navigationService, IFacade facade) : base(navigationService, facade)
        {
            this.facade = facade;
            this.navService = navigationService;
            this.vault = new PasswordVault();
            this.LoginCommand = new DelegateCommand(async()=> await LoginUser(Password,Username));
            var savedCredentials = vault.RetrieveAll();
            if (savedCredentials.Count != 0)
            {
                //check first if we already have the credetials stored, 
                //if yes make sure you change the successfully logged in message
                var credetialsRetrieved = savedCredentials[0];
                this.Password = credetialsRetrieved.Password;
                this.Username = credetialsRetrieved.UserName;
                ApplicationDataCompositeValue composite = (ApplicationDataCompositeValue)localSettings.Values[PreferencesEnum.LockerData];
                if (composite !=null)
                {
                    Constants.DeviceId = composite[PreferencesEnum.LockerDeviceName].ToString();
                    Constants.IotHubConnectionString = composite[PreferencesEnum.LockerIotHubConnectionString].ToString();
                }
                this.LoginUser(Password, Username);

            }

        }

        private async Task InitializeLocker(string password, string username)
        {
           var lockerId ="Locker"+PinGenerator.GeneratePin();
           var lockerInit = await this.facade.AddNewLocker(lockerId);
            if (lockerInit.IsSuccessful)
            {
                var lockerData = lockerInit.Content;
                ApplicationDataCompositeValue composite =new ApplicationDataCompositeValue();
                composite[PreferencesEnum.LockerDeviceName] = lockerData.DeviceId;
                composite[PreferencesEnum.LockerIotHubConnectionString] = lockerData.ConnectionString;
                composite[PreferencesEnum.LockerSymmetricKey] = lockerData.SymmetricKey;
                localSettings.Values[PreferencesEnum.LockerData] = composite;
                await this.LoginUser(password, username);
            }
            else
            {
                var dialogResult = this.DisplayDialog("Error", "Couldn't register locker. Please check your internet conenction", 1, "OK", null);
                return;
            }
        }

        public async Task LoginUser(string pass, string username)
        {
          
          
            if (pass.Length == 0 || username.Length == 0)
            {
               var dialogResult= this.DisplayDialog("Error", "All fields are mandatory", 1, "OK",null);
                return;
            }
            var loginResult = await this.facade.LoginUser(pass, username);
            if (loginResult.IsSuccessful)
            {
                var savedCredentials = vault.RetrieveAll();
                if (savedCredentials.Count != 0)
                {
                    Constants.Token = loginResult.Content.Token;
                    var dialogResult = this.DisplayDialog("Successful", " Auto Logging has been successful", 1, "OK", null);
                    navService.Navigate(Constants.NavigationPages.MainPage, null);
                }
                else
                {
                    PasswordCredential credentials = new PasswordCredential();
                    credentials.Resource = PreferencesEnum.AppCredentials;
                    credentials.Password = pass;
                    credentials.UserName = username;
                    Constants.Token = loginResult.Content.Token;
                    vault.Add(credentials);
                    var dialogResult = this.DisplayDialog("Linked", "The account has been linked to this locker.", 1, "OK", null);
                    navService.Navigate(Constants.NavigationPages.MainPage, null);
                }
               
            }
            else
            {
                var dialogResult = this.DisplayDialog("Unsuccessful login", "Login failed. Please check the credentials and try again", 1, "OK", null);

            }

        }

    }
}
