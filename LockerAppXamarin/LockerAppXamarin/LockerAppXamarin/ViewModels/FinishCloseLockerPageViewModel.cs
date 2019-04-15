using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LockerAppXamarin.ViewModels
{
	using System.Threading;
	using System.Threading.Tasks;
	using Interfaces;
	using Prism.Navigation;
	using Views;
	using Xamarin.Forms;

	public class FinishCloseLockerPageViewModel : ViewModelBase
	{
		private INavigationService _navService;
		private int _counter;


		public string ByeMessage => "Going Back to Main Menu";

		public bool ShouldShowByeMessage { get; set; }
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
			ShouldShowByeMessage = false;
			Counter = 30;
			this._navService = navigationService;
            var timerThread=new Thread(InitializeTimer);
            timerThread.Start();
			

		}

		public async void InitializeTimer()
		{
			while (Counter != 0)
			{
				this.Counter--;
                Thread.Sleep(1000);

				if (Counter < 4 && ShouldShowByeMessage==false)
				{
					ShouldShowByeMessage = true;
				}
            }

			await this._navService.NavigateAsync(nameof(MainPage));

		}

	}
}
