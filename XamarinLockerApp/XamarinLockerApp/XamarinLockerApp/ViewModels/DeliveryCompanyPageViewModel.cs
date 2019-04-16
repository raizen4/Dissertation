namespace XamarinLockerApp.ViewModels
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Interfaces;
    using Prism.Commands;
    using Prism.Navigation;
    using Prism.Services;

    class DeliveryCompanyPageViewModel:ViewModelBase
    {
        private readonly INavigationService _navService;
        private List<string> _deliveryCompanies;
        private string _companySelected;
        private bool _anyItemSelected;
        

        public DelegateCommand FinishCommand { get; set; }

        public DelegateCommand CancelCommand { get; set; }
        public bool AnyItemSelected
        {
            get => this._anyItemSelected;
            set
            {
                this._anyItemSelected = value;
                RaisePropertyChanged();
            }
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
            FinishCommand=new DelegateCommand(async()=>await this.FinishSelection());
            CancelCommand = new DelegateCommand(async () => await this.CancelPopup());

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


        private async Task CancelPopup()
        {
            await this._navService.ClearPopupStackAsync();
        }

        public async Task FinishSelection()
        {

            NavigationParameters parameters=new NavigationParameters();
            parameters.Add("COMPANY",CompanySelected);
            await this._navService.ClearPopupStackAsync(parameters);
        }

       
    }
}
