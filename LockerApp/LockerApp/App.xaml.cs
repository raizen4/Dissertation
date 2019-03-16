using System;
using System.Threading.Tasks;
using LockerApp.Interfaces;
using LockerApp.Services;
using LockerApp.Views;
using Microsoft.Practices.Unity;
using Prism.Mvvm;
using Prism.Unity.Windows;
using Prism.Windows.AppModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.Security.Credentials;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LockerApp
{
    /// <summary>

    /// Provides application-specific behavior to supplement the default Application class.

    /// </summary>

    public sealed partial class App : PrismUnityApplication
    {

        private PasswordVault vault;

        public App()
        {

            InitializeComponent();

        }





        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
          
               NavigationService.Navigate(Constants.NavigationPages.GettingStartedPage, null);

          

            Window.Current.Activate();

            return Task.FromResult<object>(null);
        }

        protected override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            RegisterTypes();
            return base.OnInitializeAsync(args);
        }

        private void RegisterTypes()
        {
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var viewModelTypeName = string.Format(System.Globalization.CultureInfo.InvariantCulture, "LockerApp.ViewModels.{0}ViewModel, LockerApp", viewType.Name);
                var viewModelType = Type.GetType(viewModelTypeName);
                return viewModelType;
            });
            
            Container.RegisterType<IFacade, Facade>();
            Container.RegisterType<IApiWrapper, ApiWrapper>();


        }


    }
}
