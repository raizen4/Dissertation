using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinLockerApp.ServiceModels
{
    using Enums;

   public class NewPowerStatusRequest
    {
        public PowerTypeEnum PowerStatus { get; set; }
    }
}
