using System;
using System.Collections.Generic;
using System.Text;

namespace LockerApp
{
    using Microsoft.Azure.Devices.Client;
    using Models;

    static class Constants
    {

        public static Locker UserLocker{ get; set; }

        public static string LockerConnectionString =
            "";
        public static string Token ="";
        public static  string WebApiEndpoint="http://192.168.88.30:4000";
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
            public const string PinAcceptedPage = "Finish";
            public const string InfoPage = "LockerInfo";

        }

    }
}
