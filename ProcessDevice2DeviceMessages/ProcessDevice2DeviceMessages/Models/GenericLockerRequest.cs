using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.ServiceModels
{
    using Enums;
    

    public class GenericLockerRequest
    {
        public LockerActionEnum Action { get; set; }
        public string SenderDeviceId { get; set; }

        public string TargetedDeviceId { get; set; }
        public string IotHubEndpoint { get; set; }
    }
}
