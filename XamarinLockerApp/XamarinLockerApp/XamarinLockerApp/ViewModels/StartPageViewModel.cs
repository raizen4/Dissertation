namespace XamarinLockerApp.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using Enums;
    using Helpers;
    using Interfaces;
    using Models;
    using Prism.Commands;
    using Prism.Navigation;
    using Prism.Services;
    using Views;
    using Xamarin.Essentials;
    using Xamarin.Forms;

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
         
            LoginCommand = new DelegateCommand(async()=> await this.LoginUser(Password,Username));
          
            if (SecureStorage.GetAsync("PASSWORD").GetAwaiter().GetResult() != null)
            {
                //check first if we already have the credetials stored, 
                //if yes make sure you change the successfully logged in message

                 Password =  SecureStorage.GetAsync("PASSWORD").GetAwaiter().GetResult(); 
                 Username = SecureStorage.GetAsync("USERNAME").GetAwaiter().GetResult();
                 var autoLoginCommand = new Command(async () => await LoginUser(Password, Username));
                 autoLoginCommand.Execute(null);


            }
          

        }


     


        private async Task<bool> InitializeLocker(string pass, string user)
        {
           var lockerId ="Locker"+PinGenerator.GeneratePin();
            IsLoading = true;
           var lockerInit = await this._facade.AddNewLocker(lockerId);
            IsLoading = false;
            if (lockerInit.HasBeenSuccessful)
            {
                Constants.UserLocker = lockerInit.Content;
                return true;
            }
            await this._dialogService.DisplayAlertAsync("Error",
                "Couldn't register locker. Please try again", "OK"
            );
          
            return false;

        }

        public async Task LoginUser(string pass, string user)
        {
           

                if (pass.Length == 0 || user.Length == 0)
            {
                await this._dialogService.DisplayAlertAsync("Error",
                    "All fields are mandatory", "OK"
                );
             
                return;
            }

            IsLoading = true;
            var loginResult = await this._facade.LoginUser(pass, user);
            IsLoading = false;
            if (loginResult.HasBeenSuccessful)
            {
                Constants.Token = loginResult.Content.Token;
                Constants.UserLocker = loginResult.Content;
             
                if (await SecureStorage.GetAsync("PASSWORD")!=null)
                {

                
                  
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
                    await this._dialogService.DisplayAlertAsync("Linked",
                        "The account has been linked to this locker.", "OK"
                    );
                 
                    
                   await this._navService.NavigateAsync(nameof(MainPage), null);
                }
               
            }
            else
            {
                await this._dialogService.DisplayAlertAsync("Unsuccessful login",
                    "Login failed. Please check the credentials and try again", "OK"
                );
             
               

            }

        }

       
       
    }
}
