using System;
using System.Collections.Generic;
using System.Text;

namespace LockerApp.ServiceModels
{
    using Enums;
    using LockerApp.Models;

    public class LockerMessage
    {
        public LockerActionRequestsEnum ActionRequest { get; set; }
        public LockerActionRequestsEnum ActionResult { get; set; }

        public string SenderDeviceId { get; set; }
        public bool HasBeenSuccessful { get; set; }
      
        public string TargetedDeviceId { get; set; }
        public string IotHubEndpoint { get; set; }

      
    }
}
