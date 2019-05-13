using System;
using System.Collections.Generic;
using System.Text;

namespace ProcessDevice2DeviceMessages
{
    public static class Constants
    {
        public static string UserApiEndpoint ="http://192.168.88.30:4000";

        public class Headers
        {
            /// <summary>
            /// The type of the content.
            /// </summary>
            public const string ContentType = "application/json";
        }

        public static string IotHubConnectionString = "HostName=Dissertation-IotHub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=OM1nr8BQfYrrUGk9gMEC8P29t0tqBIxgXGvTYgeUMT4=";
    }
}
