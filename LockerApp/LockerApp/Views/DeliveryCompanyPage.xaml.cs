using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LockerApp.Views
{
    using System.ServiceModel.Channels;
    using ViewModels;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DeliveryCompanyPage : ContentDialog
    {
        private DeliveryCompanyPageViewModel viewModel;
        ContentDialog contentDialog;

        public DeliveryCompanyPage()
        {      
            this.InitializeComponent();
            this.viewModel = DataContext as DeliveryCompanyPageViewModel;
            this.contentDialog = this;
            this.contentDialog.IsPrimaryButtonEnabled =false;
        }

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {

            var company = e.ClickedItem as string;
            if (company == this.viewModel.CompanySelected)
            {
                this.contentDialog.IsPrimaryButtonEnabled = false;

            }
            else
            {
                this.contentDialog.IsPrimaryButtonEnabled = true;

            }

            this.viewModel.CompanySelected = company;
          
        }

      
          
           
        }
    }
 

