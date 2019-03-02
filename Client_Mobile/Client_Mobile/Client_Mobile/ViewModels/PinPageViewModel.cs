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
            FinishCommand = new DelegateCommand(SendPin);
            PinCode = PinGenerator.GeneratePin();
        }

        /// <inheritdoc />
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            ForCourier = parameters.GetValue<bool>(LockerAccessEnum.Courier.ToString());
            ForFriend = parameters.GetValue<bool>(LockerAccessEnum.Friend.ToString());
        }

        private async void SendPin()
        {
            if (ForCourier)
            {
                FinishCourier();
            }
            else
            {
                FinishFriend();
            }
        }

        private async void FinishFriend()
        {
            IsLoading = true;
            if (FriendSms.Length == 0 && FriendEmail.Length == 0)
            {
                await this._dialogService.DisplayAlertAsync("Error",
                    "Email and SMS fields are both empty. Please fill at least one and try again", "OK");
            }
            else
            {
                var newPin = new Pin();
                newPin.Code = PinCode;
                newPin.PickerType = PickerTypeEnum.Friend;
                newPin.PickerDetails.Email = FriendEmail;
                newPin.PickerDetails.Phone = FriendSms;
                var apiResult = await this._facade.AddPinForLocker(newPin);
                if (apiResult.IsSuccessful)
                {
                    await this._dialogService.DisplayAlertAsync("Synced", "The pin has been synced with the servers",
                        "OK");
                    await this._navService.NavigateAsync(nameof(Views.MainPage));
                }
                else
                {
                    var dialogResult = await this._dialogService.DisplayAlertAsync("Error",
                        "Couldn't sync with the servers. Please try again",
                        "OK", "Cancel");
                    if (dialogResult)
                    {
                        FinishFriend();
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        private async void FinishCourier()
        {
            IsLoading = true;
            var newPin = new Pin();
            newPin.Code = PinCode;
            newPin.PickerType = PickerTypeEnum.Courier;
            var apiResult = await this._facade.AddPinForLocker(newPin);
            if (apiResult.IsSuccessful)
            {
                await this._dialogService.DisplayAlertAsync("Synced",
                    "The pin has been synced with the servers",
                    "OK");
                await this._navService.NavigateAsync(nameof(Views.MainPage));
            }
            else
            {
                var dialogResult = await this._dialogService.DisplayAlertAsync("Error",
                    "Couldn't sync with the servers. Please try again",
                    "OK", "Cancel");
                if (dialogResult)
                {
                    FinishFriend();
                }
                else
                {
                    return;
                }
            } 
        }
    }
}