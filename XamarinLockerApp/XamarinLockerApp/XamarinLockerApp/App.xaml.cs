using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XamarinLockerApp
{
    using Interfaces;
    using Prism;
    using Prism.Ioc;
    using Prism.Plugin.Popups;
    using Services;
    using ViewModels;
    using Views;
    using Xamarin.Forms;

    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/StartPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
           
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterPopupNavigationService();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<FinishOpenLockerPage, FinishOpenLockerPageViewModel>();
            containerRegistry.RegisterForNavigation<FinishCloseLockerPage, FinishCloseLockerPageViewModel>();
            containerRegistry.RegisterForNavigation<StartPage, StartPageViewModel>();
            containerRegistry.RegisterForNavigation<LockerInfoPage, LockerInfoPageViewModel>();
            containerRegistry.RegisterForNavigation<DeliveryCompanyPage, DeliveryCompanyPageViewModel>();  
            containerRegistry.Register<IFacade, Facade>();
            containerRegistry.Register<IApiWrapper, ApiWrapper>();
            containerRegistry.Register<ILockerPiApiWrapper,LockerApiWrapper>();
            containerRegistry.Register<IIoTHub,IoTHub>();

          
        }
    }
}
