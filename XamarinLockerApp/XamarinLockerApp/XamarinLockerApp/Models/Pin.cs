namespace XamarinLockerApp.Models
{
    using Enums;

    public class Pin
    {
        private string _code;
        private PickerTypeEnum _userType;
        private ContactDetails _parcelContactDetails;
        private string _created;


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

        public PickerTypeEnum UserType
        {
            get => this._userType;
            set => this._userType = value;
        }

        public string Created
        {
            get => this._created;
            set => this._created = value;
        }


    }
}
