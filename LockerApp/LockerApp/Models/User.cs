using System;
using System.Collections.Generic;
using System.Text;

namespace LockerApp.Models
{
    public class User
    { 

        private string _profileName;
        private string _lockerId;
        private string _token;


        public string Token
        {
            get => this._token;
            set => this._token = value;
        }
        public string ProfileName
        {
            get => this._profileName;
            set => this._profileName = value;
        }

        public string LockerId
        {
            get => this._lockerId;
            set => this._lockerId = value;
        }




    }
}
