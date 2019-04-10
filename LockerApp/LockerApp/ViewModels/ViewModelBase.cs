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
    using MvvmDialogs;

    public class ViewModelBase :Prism.Windows.Mvvm.ViewModelBase
    {
        public INavigationService NavigationService { get;  set; }
        public IFacade Facade { get;  set; }


        private bool _isLoading;
      

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

      

        
    }
}
