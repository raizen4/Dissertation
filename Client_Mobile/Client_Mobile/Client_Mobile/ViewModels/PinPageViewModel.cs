using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Client_Mobile.ViewModels
{
	using System.Threading;
    using System.Threading.Tasks;
    using Enums;
	using Helpers;
	using Interfaces;
	using Models;
	using Prism.Navigation;
	using Prism.Services;

	public class PinPageViewModel : ViewModelBase
	{
		private bool _forCourier;
		private bool _forFriend;
		private string _friendEmail;
		private string _friendSms;
		private string _pinCode;

        private readonly IFacade _facade;
		private readonly INavigationService _navService;
		private readonly IPageDialogService _dialogService;
		

		public DelegateCommand FinishCommand { get; set; }

		public bool ForCourier
		{
			get => this._forCourier;
			set => this._forCourier = value;
		}

		public bool ForFriend
		{
			get => this._forFriend;
			set => this._forFriend = value;
		}
		public string FriendSms
		{
			get => this._friendSms;
			set => this._friendSms = value;
		}
		public string FriendEmail
		{
			get => this._friendEmail;
			set => this._friendEmail = value;
		}

		public string PinCode
		{
			get => this._pinCode;
			set => this._pinCode = value;
		}

        public PinPageViewModel(INavigationService navigationService, IFacade facade,
			IPageDialogService dialogService)
			: base(navigationService)
        {
	        this._dialogService = dialogService;
	        this._navService = navigationService;
	        this._facade = facade;
			FinishCommand= new DelegateCommand(Finish);
	        PinCode = PinGenerator.GeneratePin();


        }

		/// <inheritdoc />
		public override void OnNavigatedTo(INavigationParameters parameters)
		{
			base.OnNavigatedTo(parameters);
			ForCourier = parameters.GetValue<bool>(LockerAccessEnum.Courier.ToString());
			ForFriend = parameters.GetValue<bool>(LockerAccessEnum.Friend.ToString());

        }

		private async void Finish()
		{
			IsLoading = true;
			if (FriendSms.Length == 0 && FriendEmail.Length == 0)
			{
				await this._dialogService.DisplayAlertAsync("Error",
					"Email and SMS fields are both empty. Please fill at least one and try again", "OK");
				
			}
            else
			{
				var newPin=new Pin();
				newPin.Code = PinCode;
				var lockerResult = await this._facade.SendPinToLocker(newPin);
				if (lockerResult)
				{
					var lockerSentBackResponse = await CheckForLockerResponse();
                    if (lockerSentBackResponse)
					{
						var apiResult = await this._facade.AddPinForLocker(newPin);
						if (apiResult.IsSuccessful)
						{
							await this._dialogService.DisplayAlertAsync("Synced", "The pin has been synced with the servers",
								"OK");
                        }
						else
						{
							var dialogResult=await this._dialogService.DisplayAlertAsync("Error", "Couldn't sync with the servers. Please try again",
								"OK", "Cancel");
							if (dialogResult)
							{
								Finish();

							}
							else
							{
								return;
							}
                        }
                    }
                  
				}
				await this._dialogService.DisplayAlertAsync("No Response", " The locker didn't send back the acknowledgement. Please try again",
					"OK");
				
			}
		}

		private async Task<bool> CheckForLockerResponse()
		{
			var lockerSentBackResponse = false;
			var i = 0;
			while (i < 15)
			{
				var newMesasgeReceived = await this._facade.GetPendingMessagesFromHub();
				if (newMesasgeReceived != null)
				{
					string stringifiedAction;
					int normalizedAction = 0;

					try
					{
						newMesasgeReceived.Properties.TryGetValue("Action", out stringifiedAction);
						normalizedAction = int.Parse(stringifiedAction);
					}
					catch (Exception e)
					{
						//silently fail
					}

					if (normalizedAction == 0)
					{
						//
					}
					else if (normalizedAction == (int)LockerActionEnum.NewPinGenerated)
					{
						await this._dialogService.DisplayAlertAsync("Successful", " New pin has been added to the locker.",
							"OK");
						return true;

                    }

					i++;
					Thread.Sleep(1000);
				}
			}

			return false;
        
	}
	
}
}
