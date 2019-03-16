using System;
using System.Collections.Generic;
using System.Text;

namespace ProcessDevice2DeviceMessages
{
    public static class Constants
    {
        public static string UserApiEndpoint = "HostName=Dissertation-IotHub.azure-devices.net;DeviceId=DeviceExplorerTwinSimulator;SharedAccessKey=KOGiKsfUuV+QTTAx3MKInhlBYKhiYYETQ66LXpyyzXw=";

        public class Headers
        {
            /// <summary>
            /// The type of the content.
            /// </summary>
            public const string ContentType = "application/json";
        }

        public static string IotHubConnectionString = "HostName=Dissertation-IotHub.azure-devices.net;DeviceId=DeviceExplorerTwinSimulator;SharedAccessKey=KOGiKsfUuV+QTTAx3MKInhlBYKhiYYETQ66LXpyyzXw=";
    }
}
