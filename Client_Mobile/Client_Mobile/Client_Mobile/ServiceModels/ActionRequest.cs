using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.ServiceModels
{
    using Client_Mobile.Models;
    using Enums;

    class ActionRequest
    {
        public LockerActionEnum Action { get; set; }
        public Pin Pin { get; set; } 


    }
}
