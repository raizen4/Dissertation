using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Models
{
    public class Parcel
    {
        private string id;
        private string dateDelivered;
        private string courierCompany;

        public string Id
        {
            get => this.id;
            set => this.id = value;
        }

        public string DateDelivered
        {
            get => this.dateDelivered;
            set => this.dateDelivered = value;

        }

        public string CourierCompany
        {
            get => this.courierCompany;
            set => this.courierCompany = value;
        }
    }
}
