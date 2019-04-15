using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockerApp.ViewModels
{
    using System.Runtime.CompilerServices;
    using Windows.UI.Popups;
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
                   
                    RaisePropertyChanged();

                }
                else
                {
                    this.companySelected = "";
                  
                }



            }
        }

       
      
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

       
    }
}
