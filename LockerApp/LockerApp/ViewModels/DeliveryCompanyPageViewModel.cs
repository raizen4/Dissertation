using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockerApp.ViewModels
{
    using System.Runtime.CompilerServices;
    using Interfaces;
    using MvvmDialogs;
    using Prism.Commands;
    using Prism.Windows.Navigation;

    class DeliveryCompanyPageViewModel:ViewModelBase
    {
        private readonly INavigationService navService;
        private readonly IFacade facade;
        private readonly IDialogService dialogService;
        private List<string> deliveryCompanies;
        private string companySelected;
        private bool anyItemSelected;


        public bool AnyItemSelected
        {
            get => this.anyItemSelected;
            set
            {
                this.anyItemSelected = value;
                RaisePropertyChanged("AnyItemSelected");
            }

       
        }
       
        public string CompanySelected
        {
            get => this.companySelected;
            set
            {
                if (value == null)
                {
                    return;
                }



                if (this.companySelected != value)
                {
                    this.companySelected = value;
                    AnyItemSelected = true;    
                    RaisePropertyChanged();

                }
                else
                {
                    this.companySelected = "";
                    AnyItemSelected = false;
                }



            }
        }

       
        public DelegateCommand FinalUserChoiceCommand { get; set; }
        public DelegateCommand CancelPressedCommand { get; set; }
        public List<string> DeliveryCompanies
        {
            get => this.deliveryCompanies;
            
            set => this.deliveryCompanies = value;
        }

        /// <inheritdoc />
        public DeliveryCompanyPageViewModel(INavigationService navigationService, IFacade facade, IDialogService dialogService) : base(navigationService, facade)
        {
            
            this.navService = navigationService;
            this.facade = facade;
            this.dialogService = dialogService;
            FinalUserChoiceCommand=new DelegateCommand(Finish);
            CancelPressedCommand = new DelegateCommand(()=>
            {
                this.navService.Navigate(Constants.NavigationPages.GettingStartedPage, null);
                
            });



            DeliveryCompanies = new List<string>()
            {
                "AMAZON",
                "DHL",
                "CARGUS",
                "ROYAL MAIL",
                "DPD",
                "PARCEL FORCE",
                 "OTHER"
            };
        }

        public void Finish()
        {
            var dataToSendBack = this.CompanySelected;
            this.navService.Navigate(Constants.NavigationPages.GettingStartedPage, dataToSendBack);
            
        }
    }
}
