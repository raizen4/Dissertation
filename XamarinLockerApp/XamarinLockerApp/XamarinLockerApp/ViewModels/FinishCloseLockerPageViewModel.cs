namespace XamarinLockerApp.ViewModels
{
	using System.Threading;
	using Interfaces;
	using Prism.Navigation;
	using Views;

	public class FinishCloseLockerPageViewModel : ViewModelBase
	{
		private INavigationService _navService;
		private int _counter;


		public int Counter
		{
			get => this._counter;
			set
			{
				this._counter = value;
                RaisePropertyChanged();
			}
		}

		/// <inheritdoc />
		public FinishCloseLockerPageViewModel(INavigationService navigationService, IFacade facade) : base(navigationService, facade)
		{
			
			Counter = 100;
			this._navService = navigationService;
			InititalizeTimer();



		}

	
		public void InititalizeTimer()
		{
			var timerThread = new Thread(Run);
			timerThread.Start();
        }
		private async void Run()
		{
			Thread.Sleep(2000);
			while (Counter != 0)
			{
				this.Counter--;
                Thread.Sleep(1000);

			
            }

			await this._navService.NavigateAsync(nameof(MainPage));

		}

	}
}
