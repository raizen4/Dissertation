namespace XamarinLockerApp.ViewModels
{
    using System.Threading;
    using Interfaces;
    using Prism.Commands;
    using Prism.Navigation;
    using Views;

    class FinishOpenLockerPageViewModel : ViewModelBase
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

        private int _counter;
        private Timer _timer;
        private INavigationService _navService;
        private IFacade facade;



        public FinishOpenLockerPageViewModel(INavigationService navigationService, IFacade facade) : base(navigationService, facade)
        {
            this._navService = navigationService;

     
            FinishCommand = new DelegateCommand(() =>this._navService.NavigateAsync(nameof(MainPage),null));
            this.facade = facade;
            Counter = 200;
            //InitializeTimer();

        }

        void InitializeTimer()
        {
            var timerThread = new Thread(RunTimer);
            timerThread.Start();
            
        }


        private async void RunTimer()
        {
            while (true)
            {
                if (Counter != 0)
                {
                    Thread.Sleep(2000);
                    while (Counter != 0)
                    {
                        Thread.Sleep(1000);
                        Counter--;
                    }
                }
                else
                {
                    IsLoading = true;
                    var closedLockerResult = await this.facade.CloseLocker();
                    IsLoading = false;
                    if (closedLockerResult.HasBeenSuccessful)
                    {
                        await this._navService.NavigateAsync(nameof(FinishCloseLockerPage));
                    }

                    continue;
                }

                break;
            }
        }
    }
}
