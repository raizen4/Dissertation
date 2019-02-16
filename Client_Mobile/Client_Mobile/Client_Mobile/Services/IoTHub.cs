using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Services
{
    using System.Threading.Tasks;
    using Enums;
    using Interfaces;
    using Microsoft.Azure.Devices.Client;
    using Models;
    using Newtonsoft.Json;
    using ServiceModels;
    using Xamarin.Forms;


    public class IoTHub : IIoTHub
    {

        private TransportType transportProtocol;
        private string connectionString;
        private DeviceClient deviceClient;
        private bool isConnected = false;

        /// <inheritdoc />
        ///
        ///
        ///
        ///
        ///

        public IoTHub(string connectionString, TransportType transportType)
        {
            this.transportProtocol = TransportType.Amqp;
            this.connectionString = connectionString;
            this.deviceClient = DeviceClient.CreateFromConnectionString(this.connectionString, this.transportProtocol);
        }

        public bool IsConnectedToCloud()
        {
            return this.deviceClient != null;
        }

        /// <inheritdoc />
        public async Task<bool> LockAsync(string deviceId, LockerActionEnum action)
        {
            if (IsConnectedToCloud())
            {

                GenericLockerRequest req = new GenericLockerRequest();
                req.Action = action;
                req.DeviceId = deviceId;
                var messageString = JsonConvert.SerializeObject(req);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));
                try
                {
                    await this.deviceClient.SendEventAsync(message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }

                return true;
            }

            return false;


        }

        /// <inheritdoc />
        public async Task<bool> Unlock(string deviceId, LockerActionEnum action)
        {
            if (IsConnectedToCloud())
            {

                GenericLockerRequest req = new GenericLockerRequest();
                req.Action = action;
                req.DeviceId = deviceId;
                var messageString = JsonConvert.SerializeObject(req);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));
                try
                {
                    await this.deviceClient.SendEventAsync(message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public async Task<bool> SendPinToLocker(string deviceId, LockerActionEnum action, Pin pin)
        {
            if (IsConnectedToCloud())
            {
                PayloadLockerRequest<Pin> req=new PayloadLockerRequest<Pin>();
                req.Action = action;
                req.DeviceId = deviceId;
                req.Payload = pin;
                var messageString = JsonConvert.SerializeObject(req);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));
                try
                {
                    await this.deviceClient.SendEventAsync(message);
                }
                catch (Exception e)
                {
                   Console.WriteLine(e.Message);
                    return false;
                }
                
                return true;
            }

            return false;
        }
    }
}
