using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile
{
    using Microsoft.Azure.Devices.Client;
    using Models;

    static class Constants
    {
        public static string IotHubConnectionString= "";
        public static User CurrentLoggedInUser = null;
        public static  string WebApiEndpoint="http://10.0.2.2:4000";

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
