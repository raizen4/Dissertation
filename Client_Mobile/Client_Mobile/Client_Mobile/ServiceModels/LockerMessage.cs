using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.ServiceModels
{
    using Client_Mobile.Models;
    using Enums;
    using Xamarin.Forms;

    public class LockerMessage
    {
        public LockerActionEnum Action { get; set; }
        public string SenderDeviceId { get; set; }

        public string TargetedDeviceId { get; set; }
        public string IotHubEndpoint { get; set; }

        public Pin Payload { get; set; }
    }
}
