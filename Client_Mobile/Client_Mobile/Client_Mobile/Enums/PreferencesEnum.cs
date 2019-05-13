using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Enums
{
    public static class PreferencesEnum
    {
        public static string IoTHubConnectionString = "IoTHubConnectionString";
        public static string DeviceName = "DeviceName";
        public static string Email = "Email";
        public static string Password = "Password";

        public static string SymmetricKey { get; internal set; }
    }
}
