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
        public static  string WebApiEndpoint="http://192.168.88.30:4000";

        public static string DeviceName { get;  set; }
        public static string Token { get;  set; }

        public class Headers
        {
            /// <summary>
            /// The type of the content.
            /// </summary>
            public const string ContentType = "application/json";
        }

    }
}
