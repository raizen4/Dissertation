﻿namespace XamarinLockerApp.ViewModels
{
    using System;
    using Interfaces;
    using Prism.Commands;
    using Prism.Navigation;
    using Views;

    class LockerInfoPageViewModel : ViewModelBase
    {
        private INavigationService _navService;
        private IFacade _facade;
        private string _lockerId;
        private string _lockerConnectionString;


        public string LockerId
        {
            get => this._lockerId;
            set => this._lockerId = value;
        }

        public string LockerConnectionString
        {
            get => this._lockerConnectionString;
            set => this._lockerConnectionString = value;
        }

        public DelegateCommand GoBackToMainMenu { get; set; }

        public LockerInfoPageViewModel(INavigationService navigationService, IFacade facade ) : base(navigationService, facade)
        {
            try
            {
                LockerConnectionString = Constants.UserLocker.IotHubConnectionString;
                LockerId = Constants.UserLocker.DeviceId;
            }
            catch (Exception e)
            {
                LockerConnectionString = "Error";
                LockerId = "Error";
            }
          
            GoBackToMainMenu = new DelegateCommand(async()=>await this._navService.NavigateAsync(nameof(MainPage),null));

        }
    }
}
