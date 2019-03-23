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
    class FinishPageViewModel : ViewModelBase
    {

        public DelegateCommand FinishCommand { get; set; }
        public bool ShowGoodbayMessage
        {
            get => this.showGoodbayMessage;
            set => this.showGoodbayMessage = value;
        }
        public int Counter
        {
            get => this.counter;
            set => this.counter = value;
        }

        private bool showGoodbayMessage;
        private int counter;
        private DispatcherTimer timer;
        private IFacade facade;
        private INavigationService navService;

    



        public FinishPageViewModel(INavigationService navigationService, IFacade facade) : base(navigationService, facade)
        {
            this.navService = navigationService;
            this.facade = facade;
            this.FinishCommand = new DelegateCommand(() =>this.navService.Navigate(Constants.NavigationPages.MainPage,null));
            this.OpenLocker();
            this.Counter = 60;
            this.InitializeTimer();
            this.StartTimer();
          
        }

        void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += TimerTick;
        }
        void StartTimer()
        {
            timer.Start();
        }
        void TimerTick(object sender, object e)
        {
            Counter--;
            if (Counter == 5)
            {
                this.ShowGoodbayMessage = true;
            }

            if (Counter == 0)
            {
                timer.Stop();
                this.CloseLocker();
                this.FinishCommand.Execute();
            }


        }
    }
}
