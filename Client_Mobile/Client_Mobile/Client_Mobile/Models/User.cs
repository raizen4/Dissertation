using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Models
{
    public class User
    { 

        private string _profileName;
        private string _lockerId;

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
