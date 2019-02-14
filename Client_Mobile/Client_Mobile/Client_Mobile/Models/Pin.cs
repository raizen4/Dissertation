using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Models
{
    public class Pin
    {
        private string code;
        private string issuerId;
        private string ttl;

        public string Code
        {
            get => this.code;
            set => this.code = value;
        }
        public string IssuerId
        {
            get => this.issuerId;
            set => this.issuerId = value;
        }
        public string Ttl
        {
            get => this.ttl;
            set => this.ttl = value;
        }
    }
}
