namespace XamarinLockerApp.ViewModels
{
    using System.Threading;
    using Interfaces;
    using Prism.Commands;
    using Prism.Navigation;
    using Views;
    using Xamarin.Forms;

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

     
            FinishCommand = new DelegateCommand(async() =>await this._navService.NavigateAsync(nameof(FinishCloseLockerPage),null));
            this.facade = facade;
            Counter = 30;
            IsLoading = false;

            InitializeTimer();

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
                IsLoading = true;
                var openLockerResult = await this.facade.OpenLocker();
                IsLoading = false;
                if (openLockerResult.HasBeenSuccessful)
                {
                   
                        Thread.Sleep(2000);
                        while (Counter >0)
                        {
                            Thread.Sleep(1000);
                            Counter--;
                        }
                        Device.BeginInvokeOnMainThread(async()=> await this._navService.NavigateAsync(nameof(FinishCloseLockerPage))); 
                    
              
               
                }
                else
                {
                    continue;
                }

                break;
            }
        }
    }
}
