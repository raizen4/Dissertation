using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.ServiceModels
{
    using Enums;
    using Xamarin.Forms;

    public class GenericLockerRequest
    {
        public LockerActionEnum Action { get; set; }
        public string DeviceId { get; set; }

    }
}
