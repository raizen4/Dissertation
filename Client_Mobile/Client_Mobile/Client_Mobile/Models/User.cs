using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Models
{
    public class User
    { 
        private string _email;
        private string _profileId;
        private string _lockerId;

        private List<string> _lockerActivity;
        private string _token;

    

        public string Email { get => _email; set => _email = value; }

        public string ProfileId { get => _profileId; set => _profileId = value; }

        public List<string> LockerActivity { get => this._lockerActivity; set => _lockerActivity = value; }
        public string Token { get => this._token; set => _token = value; }
    }
}
