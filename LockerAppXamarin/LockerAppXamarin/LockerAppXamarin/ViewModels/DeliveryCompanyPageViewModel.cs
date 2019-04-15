using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockerAppXamarin.ViewModels
{
    using System.Runtime.CompilerServices;
    using Interfaces;
    using Prism.Commands;
    using Prism.Navigation;
    using Prism.Navigation.Xaml;
    using Prism.Services;
    using NavigationParameters = Prism.Navigation.NavigationParameters;

    class DeliveryCompanyPageViewModel:ViewModelBase
    {
        private readonly INavigationService _navService;
        private List<string> _deliveryCompanies;
        private string _companySelected;
        private bool _anyItemSelected;


        public bool AnyItemSelected
        {
            get => this._anyItemSelected;
            set => this._anyItemSelected = value;
        }
        
       
        public string CompanySelected
        {
            get => this._companySelected;
            set
            {
                if (value == null)
                {
                    return;
                }
                if (this._companySelected != value)
                {
                    this._companySelected = value;
                    AnyItemSelected = true;
                    RaisePropertyChanged();

                }
                else
                {
                    this._companySelected = "";
                    AnyItemSelected = false;

                }



            }
        }

   
        public List<string> DeliveryCompanies
        {
            get => this._deliveryCompanies;
            
            set => this._deliveryCompanies = value;
        }

        /// <inheritdoc />
        public DeliveryCompanyPageViewModel(INavigationService navigationService, IFacade facade, IPageDialogService dialogService) : base(navigationService, facade)
        {
            
            this._navService = navigationService;
           
           



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


        public async void FinishSelection()
        {

            NavigationParameters parameters=new NavigationParameters();
            parameters.Add("COMPANY",CompanySelected);
            await this._navService.ClearPopupStackAsync(parameters);
        }

       
    }
}
