using Prism;
using Prism.Ioc;
using LockerAppXamarin.ViewModels;
using LockerAppXamarin.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace LockerAppXamarin
{
    using Interfaces;
    using Prism.Plugin.Popups;
    using Services;
    using ViewModels;
    using Views;

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

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<FinishPage, FinishPageViewModel>();
            containerRegistry.RegisterForNavigation<StartPage, StartPageViewModel>();
            containerRegistry.RegisterForNavigation<LockerInfoPage, LockerInfoPageViewModel>();
            containerRegistry.RegisterForNavigation<DeliveryCompanyPage, DeliveryCompanyPageViewModel>();
            containerRegistry.RegisterPopupNavigationService();
            containerRegistry.Register<IFacade, Facade>();
            containerRegistry.Register<IApiWrapper, ApiWrapper>();

            containerRegistry.RegisterForNavigation<FinishCloseLockerPage, FinishCloseLockerPageViewModel>();
        }
    }
}
