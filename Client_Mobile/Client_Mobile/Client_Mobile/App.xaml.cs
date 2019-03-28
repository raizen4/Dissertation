using Prism;
using Prism.Ioc;
using Client_Mobile.ViewModels;
using Client_Mobile.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Client_Mobile
{
    using Client_Mobile.Enums;
    using Interfaces;
    using Prism.Navigation;
    using Services;

    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
      
        public App(IPlatformInitializer initializer) : base(initializer)
        {
           
        }
        public App()
        {
           

        }
        protected override async void OnInitialized()
        {
       
            InitializeComponent();

          var navParamas=  new NavigationParameters();
            navParamas.Add(LockerAccessEnum.Courier.ToString(), false);
            navParamas.Add(LockerAccessEnum.Friend.ToString(), true);
            await NavigationService.NavigateAsync("NavigationPage/PinPage", navParamas);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage,MainPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<RegisterPage, RegisterPageViewModel>();
            containerRegistry.RegisterForNavigation<ActivityHistoryPage, ActivityHistoryPageViewModel>();
            containerRegistry.RegisterForNavigation<PinPage, PinPageViewModel>();
            containerRegistry.Register<IFacade,Facade>();
            containerRegistry.Register<IApiWrapper, ApiWrapper>();
            containerRegistry.RegisterSingleton<IIoTHub,IoTHub>();

            containerRegistry.RegisterForNavigation<CurrentPinsPage, CurrentPinsPageViewModel>();
        }
    }
}
