using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LockerAppXamarin.Interfaces;
using Prism.Commands;

namespace LockerAppXamarin.ViewModels
{
    using System.Threading;
    using Enums;
    using Interfaces;
    using Prism.Navigation;
    using Prism.Services;
    using Views;

    class FinishPageViewModel : ViewModelBase
    {

        public DelegateCommand FinishCommand { get; set; }
      
        public int Counter
        {
            get => this._counter;
            set
            {
                this._counter = value;
                RaisePropertyChanged();
            }
        }
        public bool ShouldShowPinActionedUi { get; set; }
        public bool ShouldShowUserActionedUi { get; set; }

        public OpenLockerTypes OpenLockerType
        {
            get => this._openLockerType;
            set => this._openLockerType = value;
        }


        private int _counter;
        private OpenLockerTypes _openLockerType;
        private Timer _timer;
        private INavigationService _navService;
        private IPageDialogService _dialogService;
        private IGpioController _controller;
      



        public FinishPageViewModel(INavigationService navigationService, IFacade facade, IPageDialogService dialogService, IGpioController gpioController ) : base(navigationService, facade)
        {
            this._navService = navigationService;
            this._dialogService = dialogService;
     
            FinishCommand = new DelegateCommand(() =>this._navService.NavigateAsync(nameof(MainPage),null));
            
          
        }

        void InitializeTimer()
        {




        }



      
    }
}
