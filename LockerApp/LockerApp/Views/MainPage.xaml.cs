
using System;
using Windows.UI.Xaml.Controls;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LockerApp.Views
{
    using ViewModels;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainPageViewModel ViewModel => DataContext as MainPageViewModel;
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
