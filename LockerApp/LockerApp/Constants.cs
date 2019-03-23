using System;
using System.Collections.Generic;
using System.Text;

namespace LockerApp
{
    using Microsoft.Azure.Devices.Client;
    using Models;

    static class Constants
    {
        public static string IotHubConnectionString="HostName=Dissertation-IotHub.azure-devices.net;DeviceId=DeviceExplorerTwinSimulator;SharedAccessKey=KOGiKsfUuV+QTTAx3MKInhlBYKhiYYETQ66LXpyyzXw=";
        public static string DeviceId = "";

        public static string Token = null;
        public static  string WebApiEndpoint="http://10.2.2.0:4000";
        public class Headers
        {
            /// <summary>
            /// The type of the content.
            /// </summary>
            public const string ContentType = "application/json";
        }

        public class NavigationPages
        {
            /// <summary>
            /// The navigation.
            /// </summary>
            public const string MainPage = "Main";
            public const string GettingStartedPage = "Start";
            public const string PinAcceptedPage = "FinishPage";
            public const string InfoPage = "LockerInfoPage";

        }

    }
}
