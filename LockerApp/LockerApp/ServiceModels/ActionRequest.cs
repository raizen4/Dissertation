﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LockerApp.ServiceModels
{
    using LockerApp.Models;
    using Enums;

    class ActionRequest
    {
        public LockerActionRequestsEnum Action { get; set; }
        public Pin Pin { get; set; } 


    }
}
