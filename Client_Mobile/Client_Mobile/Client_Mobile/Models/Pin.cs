using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Models
{
    using Enums;

    public class Pin
    {
        private string _code;
        private PickerTypeEnum _pickerType;
        private ContactDetails _pickerDetails;


        public string Code
        {
            get => this._code;
            set => this._code = value;
        }

        public ContactDetails PickerDetails
        {
            get => this._pickerDetails;
            set => this._pickerDetails = value;
        }

        public PickerTypeEnum PickerType
        {
            get => this._pickerType;
            set => this._pickerType = value;
        }


    }
}
