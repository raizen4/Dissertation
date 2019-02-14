using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Models
{
    public class Profile
    {
        private string name;
        private string id;
        private List<Pin> currentActivePins;
        private List<Parcel> orderHistory;

        public string Name
        {
            get => this.name;
            set => this.name = value;
        }

        public string Id
        {
            get => this.id;
        }

        public List<Pin> CurrentActivePins
        {
            get => this.currentActivePins;
            set => this.currentActivePins = value;
        }

        public List<Parcel> OrderHistory
        {
            get => this.orderHistory;
        }

    }
}
