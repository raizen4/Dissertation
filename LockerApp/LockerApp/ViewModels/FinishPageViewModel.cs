using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LockerApp.Interfaces;
using Prism.Commands;
using Prism.Windows.Navigation;
using Windows.UI.Xaml;

namespace LockerApp.ViewModels
{
    using Enums;
    using MvvmDialogs;

    class FinishPageViewModel : ViewModelBase
    {

        public DelegateCommand FinishCommand { get; set; }
        public bool ShowGoodbayMessage
        {
            get => this._showGoodbayMessage;
            set => this._showGoodbayMessage = value;
        }
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
            get => this.openLockerType;
            set => this.openLockerType = value;
        }


        private bool _showGoodbayMessage;
        private int _counter;
        private OpenLockerTypes openLockerType;
        private DispatcherTimer _timer;
        private INavigationService _navService;
        private IDialogService _dialogService;
        private IGpioController controller;
        private bool userManuallyLockedUsingApp;


        public FinishPageViewModel(INavigationService navigationService, IFacade facade, IDialogService dialogService, IGpioController gpioController ) : base(navigationService, facade)
        {
            this._navService = navigationService;
            this._dialogService = dialogService;
            this.userManuallyLockedUsingApp = false;
            this.FinishCommand = new DelegateCommand(() =>this._navService.Navigate(Constants.NavigationPages.MainPage,null));
            this.controller = gpioController;
            this.controller.OpenLocker();
            this.InitializeTimer();
            this.StartTimer();
          
        }

        void InitializeTimer()
        {
            this.Counter = 30;
            this._timer = new DispatcherTimer();
            this._timer.Interval = new TimeSpan(0, 0, 1);
            this._timer.Tick += TimerTick;
        }
        void StartTimer()
        {
            this._timer.Start();
        }
        void TimerTick(object sender, object e)
        {
            Counter--;
            if (Counter == 5)
            {
                ShowGoodbayMessage = true;
            }

            if (Counter == 0 || this.userManuallyLockedUsingApp)
            {
                this._timer.Stop();
                this.controller.CloseLocker();
                FinishCommand.Execute();
            }


        }

        /// <inheritdoc />
        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            ShouldShowPinActionedUi = false;
            ShouldShowUserActionedUi = false;
            var openType = (OpenLockerTypes.OpenTypes) e.Parameter;
            if (openType == OpenLockerTypes.OpenTypes.UserActioned)
            {
                this.ShouldShowUserActionedUi = true;
            }
            else
            {
                this.ShouldShowPinActionedUi = true;
            }

        }
    }
}
