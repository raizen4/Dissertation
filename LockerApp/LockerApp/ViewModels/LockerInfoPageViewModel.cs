using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LockerApp.Interfaces;
using Prism.Commands;
using Prism.Windows.Navigation;

namespace LockerApp.ViewModels
{
    class LockerInfoPageViewModel : ViewModelBase
    {
        private INavigationService navService;
        private IFacade facade;
        private string lockerId;
        private string lockerConnectionString;


        public string LockerId
        {
            get => this.lockerId;
            set => this.lockerId = value;
        }

        public string LockerConnectionString
        {
            get => this.lockerConnectionString;
            set => this.lockerConnectionString = value;
        }

        public DelegateCommand GoBackToMainMenu { get; set; }

        public LockerInfoPageViewModel(INavigationService navigationService, IFacade facade) : base(navigationService, facade)
        {
            this.GoBackToMainMenu = new DelegateCommand(()=>navService.Navigate(Constants.NavigationPages.MainPage,null));

        }
    }
}
