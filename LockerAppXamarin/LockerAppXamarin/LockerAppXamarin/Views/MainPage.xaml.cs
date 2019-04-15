
using System;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LockerAppXamarin.Views
{
    using ViewModels;
    using Xamarin.Forms;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : ContentPage
    {
        private MainPageViewModel ViewModel => BindingContext as MainPageViewModel;
        public MainPage()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception ex)
            {
                // Log error (including InnerExceptions!)
                // Handle exception }
            }
        }
    }
}
