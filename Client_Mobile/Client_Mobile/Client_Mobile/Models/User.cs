using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Models
{
    public class User
    { 

        private string _profileName;
        private string _lockerId;
        private string _currentDeviceId;
        private string _iotHubConnectionString;
        public string DisplayName
        {
            get => this._profileName;
            set => this._profileName = value;
        }

        public string LockerId
        {
            get => this._lockerId;
            set => this._lockerId = value;
        }
        public string DeviceId
        {
            get => this._currentDeviceId;
            set => this._currentDeviceId = value;
        }

        public string IotHubConnectionString
        {
            get => this._iotHubConnectionString;
            set => this._iotHubConnectionString = value;

        }
    }
}
