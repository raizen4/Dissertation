using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Models
{
    public class Parcel
    {
        private string _id;
        private string _dateDelivered;
        private string _courierCompany;

        public string Id
        {
            get => this._id;
            set => this._id = value;
        }

        public string DateDelivered
        {
            get => this._dateDelivered;
            set => this._dateDelivered = value;

        }

        public string CourierCompany
        {
            get => this._courierCompany;
            set => this._courierCompany = value;
        }
    }
}
