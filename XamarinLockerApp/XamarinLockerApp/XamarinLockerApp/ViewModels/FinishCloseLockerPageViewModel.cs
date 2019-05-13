namespace XamarinLockerApp.ViewModels
{
	using System.Threading;
	using Interfaces;
	using Prism.Navigation;
	using Views;
	using Xamarin.Forms;

	public class FinishCloseLockerPageViewModel : ViewModelBase
	{
		private INavigationService _navService;
		private IFacade facade;
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
			
			Counter = 10;
			this._navService = navigationService;
			IsLoading = false;
			this.facade = facade;
			InititalizeTimer();
			



		}

	
		public void InititalizeTimer()
		{
			var timerThread = new Thread(Run);
			timerThread.Start();
        }
		private async void Run()
		{
			IsLoading = true;
			var lockerClosedResult = await this.facade.CloseLocker();
			IsLoading = false;
			if (lockerClosedResult.HasBeenSuccessful)
			{
			Thread.Sleep(2000);
			while (Counter > 0)
			{
				this.Counter--;
                Thread.Sleep(1000);

			
            }
				Device.BeginInvokeOnMainThread(async () => await this._navService.NavigateAsync(nameof(MainPage)));
            }
			else
			{
				Run();
			}
          

		}

	}
}
