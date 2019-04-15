using System;
using System.Collections.Generic;
using System.Text;

namespace LockerAppXamarin.ServiceModels
{
    public class LoginRequest
    {
        public string Password { get; set; }
        public string Email { get; set; }

        public string LoginType { get; set; }
    }
}
