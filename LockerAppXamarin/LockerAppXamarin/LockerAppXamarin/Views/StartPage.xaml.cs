

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LockerAppXamarin.Views
{
    using ViewModels;
    using Xamarin.Forms;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StartPage : ContentPage
    {

        public StartPageViewModel ViewModel;

        public StartPage()
        {
            InitializeComponent();
            this.ViewModel=BindingContext as StartPageViewModel;
        }
    }
}
