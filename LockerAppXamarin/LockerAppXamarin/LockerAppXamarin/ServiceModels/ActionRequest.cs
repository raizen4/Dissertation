using System;
using System.Collections.Generic;
using System.Text;

namespace LockerAppXamarin.ServiceModels
{
    using Models;
    using Enums;

    class ActionRequest
    {
        public LockerActionEnum Action { get; set; }
        public Pin Pin { get; set; } 


    }
}
