using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Models
{
    using Enums;

    public class Pin
    {
        private string _code;
        private PinUserTypeEnum _userType;
        private ContactDetails _parcelContactDetails;
        private bool isExtendedView = false;
        private string _dateCreated;


        public string Code
        {
            get => this._code;
            set => this._code = value;
        }

        public ContactDetails ParcelContactDetails
        {
            get => this._parcelContactDetails;
            set => this._parcelContactDetails = value;
        }

        public PinUserTypeEnum UserType
        {
            get => this._userType;
            set => this._userType = value;
        }

        public bool IsExtendedView
        {
            get => this.isExtendedView;
            set => this.isExtendedView = value;
        }

        public string DateCreated
        {
            get => this._dateCreated;
            set => this._dateCreated = value;
        }
    }
}
