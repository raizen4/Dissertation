using Client_Mobile.Interfaces;
using Client_Mobile.ServiceModels;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Client_Mobile.ViewModels
{
    using Enums;

    public class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; private set; }
        private IFacade _facade { get; set; }
        private IPageDialogService _dialogService { get; set; }

        private string _title;
        private bool _isLoading;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public bool IsLoading
        {
            get => this._isLoading;
            set => SetProperty(ref this._isLoading, value);
        }

        public ViewModelBase(INavigationService navigationService, IFacade facade, IPageDialogService dialogService)
        {
            NavigationService = navigationService;
            this._facade = facade;
            this._dialogService = dialogService;
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {
        }

        public virtual void Destroy()
        {
        }

      
    }
}