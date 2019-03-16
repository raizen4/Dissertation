using System;
using System.Collections.Generic;
using System.Text;

namespace LockerApp.ServiceModels
{
    using LockerApp.Models;
    using Enums;

    class ActionRequest
    {
        public LockerActionEnum Action { get; set; }
        public Pin Pin { get; set; } 


    }
}
