﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.ServiceModels
{
    class RegisterRequest
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }

        public string Phone { get; set; }

        public string DeviceId { get; set; }
        
    }
}
