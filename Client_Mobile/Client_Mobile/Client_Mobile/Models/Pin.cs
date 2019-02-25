using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Models
{
    public class Pin
    {
        private string _code;
        private string _ttl;

        public string Code
        {
            get => this._code;
            set => this._code = value;
        }
       
        public string Ttl
        {
            get => this._ttl;
            set => this._ttl = value;
        }
    }
}
