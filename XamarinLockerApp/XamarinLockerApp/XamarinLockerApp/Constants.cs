﻿namespace XamarinLockerApp
{
    using Models;

    static class Constants
    {
     
        public static LockerInfo UserLocker{ get; set; }


        public static string Token ="";


        public static  string UserWebApiEndpoint="http://192.168.88.30:4000";
        public static string LockerApiEndpoint = "http://192.168.88.24:5000";
        public class Headers
        {
            /// <summary>
            /// The type of the content.
            /// </summary>
            public const string ContentType = "application/json";
        }

     

    }
}
