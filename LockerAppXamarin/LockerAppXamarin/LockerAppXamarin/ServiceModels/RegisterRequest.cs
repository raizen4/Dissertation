﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LockerAppXamarin.ServiceModels
{
    class RegisterRequest
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string LockerId { get; set; }

        public string ProfileId { get; set; }

        
    }
}