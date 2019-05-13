using Xamarin.Forms;

namespace Client_Mobile.Views
{
    using Models;
    using ViewModels;

    public partial class CurrentPinsPage : ContentPage
    {
        private CurrentPinsPageViewModel vm;
        public CurrentPinsPage()
        {
            InitializeComponent();
            this.vm=BindingContext as CurrentPinsPageViewModel;
            
        }
    
        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var currentPin = e.Item as Pin;
            this.vm.ShowOrHideExtension(currentPin);
        }

       
    }
}
