using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Client_Mobile.ViewModels
{
	using System.Collections.ObjectModel;
	using System.Reactive.Linq;
	using System.Threading.Tasks;
	using Enums;
	using Interfaces;
	using Models;
	using Prism.Services;

	public class ActivityHistoryPageViewModel : ViewModelBase
	{
		private readonly IFacade _facade;
		private readonly INavigationService _navService;
		private readonly IPageDialogService _dialogService;
		private ObservableCollection<HistoryAction> _historyActions;

		public ObservableCollection<HistoryAction> HistoryActions
		{
			get => this._historyActions;
			set => this._historyActions = value;
		}

        public ActivityHistoryPageViewModel(INavigationService navigationService, IFacade facade,
			IPageDialogService dialogService)
			: base(navigationService,facade, dialogService)
        {
	        this._facade = facade;
	        this._navService = navigationService;
	        this._dialogService = dialogService;
			HistoryActions= new ObservableCollection<HistoryAction>()
			{
				new HistoryAction()
				{
					Action = LockerActionEnum.PickedUp,
					Description = "Picked up on 2019.01.07 at 19:45:20 using pin 210431"
					
				},
				new HistoryAction()
				{
					Action = LockerActionEnum.UserAppClosed,
					Description = "Closed on 2019.01.07 at 19:45:20"

				},

				new HistoryAction()
				{
				Action = LockerActionEnum.UserAppOpened,
				Description = "Opened on 2019.01.07 at 19:45:20"

			},

	        new HistoryAction()
	        {
		        Action = LockerActionEnum.Delivered,
		        Description = "Delivered on 2019.01.07 at 19:45:20 using Pin 203123"

	        }
            };
			//GetHistory();
        }


		private async void GetHistory()
		{
			IsLoading = true;
			var apiResult = await this._facade.GetDeliveryHistory();
			if (apiResult.IsSuccessful)
			{
				HistoryActions = (ObservableCollection < HistoryAction >) apiResult.Content.ToObservable();
				IsLoading = false;
			}

			var dialogResult=await this._dialogService.DisplayAlertAsync("Failed", " Something went wrong. Try again!", "OK", "Cancel");
			if (dialogResult)
			{
				GetHistory();
			}
			else
			{
				await this._navService.NavigateAsync(nameof(Views.MainPage));
			}
			
        }



    }
}
