using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Client_Mobile.Views
{
    using ViewModels;

    public partial class MainPage : ContentPage
    {
        private MainPageViewModel viewModel;
        public MainPage()
        {
            InitializeComponent();
            this.viewModel=BindingContext as MainPageViewModel;
            
        }

        protected override bool OnBackButtonPressed()
        {



            // Begin an asyncronous task on the UI thread because we intend to ask the users permission.
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.viewModel.OnBackButtonPressed();
                if (result)
                {
                    base.OnBackButtonPressed();
                    await Navigation.PopAsync();
                }
            });

            // Always return true because this method is not asynchronous.
            // We must handle the action ourselves: see above.
            return true;

        }
    }
}