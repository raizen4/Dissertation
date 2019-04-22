using Client_Mobile.Interfaces;
using Client_Mobile.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Client_Mobile.ViewModels
{
    using System.Threading.Tasks;
    using Enums;

    public class CurrentPinsPageViewModel : ViewModelBase
    {
        private IFacade facade;
        private INavigationService navService;
        private IPageDialogService dialogService;

        private ObservableCollection<Pin> currentPins;

        public ObservableCollection<Pin> CurrentPins
        {
            get => this.currentPins;
            set
            {
                this.currentPins = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand<Pin> RemovePinCommand { get; set; }
        public DelegateCommand BackCommand { get; set; }

        public CurrentPinsPageViewModel(INavigationService navigationService, IFacade facade,
            IPageDialogService dialogService) : base(navigationService, facade, dialogService)
        {
            Title = "Active Pins";
            this.facade = facade;
            this.navService = navigationService;
            this.dialogService = dialogService;
            RemovePinCommand = new DelegateCommand<Pin>(async (pinPressed) => await RemovePin(pinPressed));
            this.BackCommand = new DelegateCommand(async () => await this.navService.NavigateAsync(nameof(Views.MainPage)));
            this.GetCurrentPins();
        }

        private async Task RemovePin(Pin pin)
        {
            var removeWarning = await this.dialogService.DisplayAlertAsync("Warning",
                "Pin number " + pin.Code + " will be removed along with all its contact details", "Ok", "Cancel");
            if (removeWarning)
            {
                IsLoading = true;
                var apiResult = await this.facade.RemovePinForLocker(pin);
                IsLoading = false;
                if (apiResult.HasBeenSuccessful)
                {
                    await this.dialogService.DisplayAlertAsync("Successful",
                        "Pin number " + pin.Code + " has been successfully deleted", "OK");
                    var currentPinIndex = CurrentPins.IndexOf(pin);
                    CurrentPins.RemoveAt(currentPinIndex);
                 
                }
                else
                {
                    var dialogResult = await this.dialogService.DisplayAlertAsync("Failed",
                        " Something went wrong when deleting this pin. Try again!", "OK", "Cancel");
                    if (dialogResult)
                    {
                        await RemovePin(pin);
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                return;
            }
        }


        private async void GetCurrentPins()
        {
            IsLoading = true;
            var apiResult = await this.facade.GetActivePins();
            IsLoading = false;
            if (apiResult.HasBeenSuccessful)
            {
                var observableList = new ObservableCollection<Pin>(apiResult.Content);
                CurrentPins = observableList;
                IsLoading = false;
                if (CurrentPins.Count == 0)
                {
                    await this.dialogService.DisplayAlertAsync("Warning", "No pins currently active.", "OK");
                    await this.navService.NavigateAsync(nameof(Views.MainPage));
                }
            }
            else
            {
                var dialogResult = await this.dialogService.DisplayAlertAsync("Failed",
                    " Something went wrong. Try again!", "OK", "Cancel");
                if (dialogResult)
                {
                    GetCurrentPins();
                }
                else
                {
                    await this.navService.NavigateAsync(nameof(Views.MainPage));
                }
            }
        }

        internal void ShowOrHideExtension(Pin pinPressed)
        {
            var currentTaskPressedIndex = CurrentPins.IndexOf(pinPressed);
            var pinHasPinPressed = pinPressed;
            if (currentTaskPressedIndex == -1)
            {
                currentTaskPressedIndex++;
            }

            pinHasPinPressed.IsExtendedView = !pinHasPinPressed.IsExtendedView;
            CurrentPins.RemoveAt(currentTaskPressedIndex);
            CurrentPins.Insert(currentTaskPressedIndex, pinHasPinPressed);
        }
    }
}