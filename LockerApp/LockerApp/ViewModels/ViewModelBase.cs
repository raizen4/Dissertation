using LockerApp.Interfaces;
using Prism.Mvvm;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace LockerApp.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware
    {
        protected INavigationService NavigationService { get; private set; }
        protected IFacade Facade { get; private set; }


        private string _title;
        private bool _isLoading;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public bool IsLoading
        {
            get => this._isLoading;
            set => SetProperty(ref this._isLoading, value);
        }

        public ViewModelBase(INavigationService navigationService,IFacade facade)
        {
            NavigationService = navigationService;
            Facade = facade;
        }

        

        public void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
        }

        public void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        {
        }

        public async Task<ContentDialogResult> DisplayDialog(string title, string content, int numberOfButtons, string closeButton, string positiveButton)
        {
            if(numberOfButtons == 1)
            {
                ContentDialog noWifiDialog = new ContentDialog
                {
                    Title = title,
                    Content = content,
                    CloseButtonText = closeButton
                };
                ContentDialogResult result = await noWifiDialog.ShowAsync();
                return result;

            }
            else
            {
                ContentDialog noWifiDialog = new ContentDialog
                {
                    Title = title,
                    Content = content,
                    PrimaryButtonText = positiveButton,
                    CloseButtonText = closeButton


                };
                ContentDialogResult result = await noWifiDialog.ShowAsync();
                return result;
            }


        }

        public void OpenLocker()
        {

        }
        public void CloseLocker()
        {

        }
    }
}
