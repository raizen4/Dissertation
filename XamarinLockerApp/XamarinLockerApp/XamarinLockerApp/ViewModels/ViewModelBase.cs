namespace XamarinLockerApp.ViewModels
{
    using System;
    using Interfaces;
    using Prism.Mvvm;
    using Prism.Navigation;

    public class ViewModelBase: BindableBase, INavigationAware, IDestructible
    {
        public INavigationService NavigationService { get;  set; }
        public IFacade Facade { get;  set; }


        private bool _isLoading;
      

        public bool IsLoading
        {
            get => this._isLoading;
            set => SetProperty(ref this._isLoading, value);
        }

        public ViewModelBase(INavigationService navigationService,IFacade facade)
        {
            NavigationService = navigationService;
            Facade = facade;


        }


        /// <inheritdoc />
        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {

        }

        /// <inheritdoc />
        public void Destroy()
        {
            throw new NotImplementedException();
        }

        
    }
}
