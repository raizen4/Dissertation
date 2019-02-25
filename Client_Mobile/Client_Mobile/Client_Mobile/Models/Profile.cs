using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Models
{
    public class Profile
    {
        private string _name;
        private string _id;
        private List<Pin> _currentActivePins;
        private List<Parcel> _orderHistory;

        public string Name
        {
            get => this._name;
            set => this._name = value;
        }

        public string Id
        {
            get => this._id;
        }

        public List<Pin> CurrentActivePins
        {
            get => this._currentActivePins;
            set => this._currentActivePins = value;
        }

        public List<Parcel> OrderHistory
        {
            get => this._orderHistory;
        }

    }
}
