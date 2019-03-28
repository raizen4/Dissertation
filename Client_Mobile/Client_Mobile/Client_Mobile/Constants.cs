﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile
{
    using Microsoft.Azure.Devices.Client;
    using Models;

    static class Constants
    {
        public static string IotHubConnectionString="HostName=Dissertation-IotHub.azure-devices.net;DeviceId=DeviceExplorerTwinSimulator;SharedAccessKey=KOGiKsfUuV+QTTAx3MKInhlBYKhiYYETQ66LXpyyzXw=";
        public static User CurrentLoggedInUser = null;
        public static  string WebApiEndpoint="http://10.2.2.0:4000";

        public static string DeviceName { get; internal set; }
        public static string Token { get; internal set; }

        public class Headers
        {
            /// <summary>
            /// The type of the content.
            /// </summary>
            public const string ContentType = "application/json";
        }

    }
}
