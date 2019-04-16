namespace XamarinLockerApp.Helpers
{
    using System;
    using System.Security.Cryptography;

    public static class PinGenerator
    {

        public static string GeneratePin()
        {
            var cryptoRng = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[sizeof(ulong)];
            cryptoRng.GetBytes(buffer);
            var num = BitConverter.ToUInt64(buffer, 0);
            var pin = num % 100000;
            return pin.ToString("D5");
        }
    }
}
