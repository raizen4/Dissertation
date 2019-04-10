﻿using Client_Mobile.Interfaces;
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
        public CurrentPinsPageViewModel(INavigationService navigationService, IFacade facade, IPageDialogService dialogService) : base(navigationService, facade, dialogService)
        {
            Title = "Active Pins";
            this.facade = facade;
            this.navService = navigationService;
            this.dialogService = dialogService;
            this.GetCurrentPins();
        }


        private async void GetCurrentPins()
        {
            IsLoading = true;
            var apiResult = await this.facade.GetActivePins();
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
                var dialogResult = await this.dialogService.DisplayAlertAsync("Failed", " Something went wrong. Try again!", "OK", "Cancel");
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
