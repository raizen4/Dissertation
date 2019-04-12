using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LockerApp.Helpers;
using LockerApp.Enums;
using LockerApp.Interfaces;
using Prism.Commands;
using Prism.Windows.Navigation;
using Windows.Security.Credentials;
using Windows.Storage;

namespace LockerApp.ViewModels
{
    using System.ComponentModel;
    using Windows.UI.Popups;
    using Windows.UI.Xaml.Controls;
    using MvvmDialogs;
    using Views;

    public class StartPageViewModel :ViewModelBase
    {
        private readonly IFacade _facade;
        private readonly INavigationService _navService;
        private readonly IDialogService _dialogService;
        private PasswordVault _vault;
        private string _password;
        private string _username;
      //  private IDialogService dialogService;
        ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;



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

        public StartPageViewModel(INavigationService navigationService, IFacade facade, IDialogService dialogService) : base(navigationService,facade)
           {
           this._dialogService = dialogService;
            this._facade = facade;
            this._navService = navigationService;
            this._vault = new PasswordVault();
            LoginCommand = new DelegateCommand(TextContentDialog);
            var savedCredentials = this._vault.RetrieveAll();
            if (savedCredentials.Count != 0)
            {
                //check first if we already have the credetials stored, 
                //if yes make sure you change the successfully logged in message
                var credetialsRetrieved = savedCredentials[0];
                Password = credetialsRetrieved.Password;
                Username = credetialsRetrieved.UserName;
              
                LoginUser(Password, Username);

            }
          

        }

        private async void TextContentDialog()
        {
            var viewModel=new DeliveryCompanyPageViewModel(this._navService,this._facade,this._dialogService);
            await this._dialogService.ShowContentDialogAsync(viewModel);
           
           
        }

         
        private async Task<bool> InitializeLocker(string pass, string user)
        {
           var lockerId ="Locker"+PinGenerator.GeneratePin();
           var lockerInit = await this._facade.AddNewLocker(lockerId);
            if (lockerInit.IsSuccessful)
            {
                return true;
            }
                await this._dialogService.ShowMessageDialogAsync("Couldn't register locker. Please try again", "Error", new[]
                {
                    new UICommand { Label = "OK" },

                });
            return false;

        }

        public async Task LoginUser(string pass, string user)
        {
           

                if (pass.Length == 0 || user.Length == 0)
            {
                await this._dialogService.ShowMessageDialogAsync("All fields are mandatory", "Error", new[]
                {
                    new UICommand { Label = "OK" },

                });
              
                return;
            }
            var loginResult = await this._facade.LoginUser(pass, user);
            if (loginResult.IsSuccessful)
            {
                var savedCredentials = this._vault.RetrieveAll();
                if (savedCredentials.Count != 0)
                {
                    Constants.Token = loginResult.Content.Token;
                    Constants.UserLocker = loginResult.Content;
                    await this._dialogService.ShowMessageDialogAsync("Auto Logging has been successful", "Successful", new[]
                    {
                        new UICommand { Label = "OK" },

                    });
                  
                    this._navService.Navigate(Constants.NavigationPages.MainPage, null);
                }
                else
                {               
                        var result = await InitializeLocker(pass, user);
                        if (!result)
                        {
                            return;
                        }
                    PasswordCredential credentials = new PasswordCredential();
                    credentials.Resource = PreferencesEnum.AppCredentials;
                    credentials.Password = pass;
                    credentials.UserName = user;
                    Constants.Token = loginResult.Content.Token;
                    this._vault.Add(credentials);
                    await this._dialogService.ShowMessageDialogAsync("The account has been linked to this locker.", "Linked", new[]
                    {
                        new UICommand { Label = "OK" },

                    });
                   
                    this._navService.Navigate(Constants.NavigationPages.MainPage, null);
                }
               
            }
            else
            {
                await this._dialogService.ShowMessageDialogAsync("Login failed. Please check the credentials and try again", "Unsuccessful login", new[]
                {
                    new UICommand { Label = "OK" },

                });
               

            }

        }

        /// <inheritdoc />
        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);


            string company = e.Parameter as string;
            

        }
    }
}
