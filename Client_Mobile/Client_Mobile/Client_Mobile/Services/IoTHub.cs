﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Client_Mobile.Services
{
    using System.Threading;
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
        private static DeviceClient deviceClient;
        private bool isConnected = false;

        /// <inheritdoc />
        ///
        ///
        ///
        ///
        ///

        public IoTHub()
        {
            this.transportProtocol = TransportType.Http1; ;
            deviceClient =
                    DeviceClient.CreateFromConnectionString(Constants.IotHubConnectionString, this.transportProtocol);
        
        }

        public async Task<bool> CheckConnection()
        {
                try
                {
                   await deviceClient.OpenAsync();                 
                   return true;
            }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
        }

        /// <inheritdoc />
        public async Task<bool> Lock(string deviceId, LockerActionEnum action)
        {
            if (!this.isConnected)
            {
                var recheckConn = await this.CheckConnection();
                if (recheckConn)
                {
                    this.isConnected = true;
                }
            }
            if (this.isConnected)
            {
                GenericLockerRequest req = new GenericLockerRequest();
                req.Action = action;
                req.DeviceId = deviceId;
                var messageString = JsonConvert.SerializeObject(req);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));
                try
                {
                    await deviceClient.SendEventAsync(message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    this.isConnected = false;
                    return false;
                }

                return true;
            }

            return false;


        }

        /// <inheritdoc />
        public async Task<bool> Unlock(string deviceId, LockerActionEnum action)
        {
            if (this.isConnected)
            {
                if (!this.isConnected)
                {
                    var recheckConn = await this.CheckConnection();
                    if (recheckConn)
                    {
                        this.isConnected = true;
                    }
                }
                GenericLockerRequest req = new GenericLockerRequest();
                req.Action = action;
                req.DeviceId = deviceId;
                var messageString = JsonConvert.SerializeObject(req);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));
                try
                {
                    await deviceClient.SendEventAsync(message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    this.isConnected = false;
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public async Task<bool> SendPinToLocker(string deviceId, LockerActionEnum action, Pin pin)
        {
            if (!this.isConnected)
            {
                var recheckConn = await this.CheckConnection();
                if (recheckConn)
                {
                    this.isConnected = true;
                }
            }
            if (this.isConnected)
            {
                PayloadLockerRequest<Pin> req=new PayloadLockerRequest<Pin>();
                req.Action = action;
                req.DeviceId = deviceId;
                req.Payload = pin;
                var messageString = JsonConvert.SerializeObject(req);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));
                try
                {
                    await deviceClient.SendEventAsync(message);
                }
                catch (Exception e)
                {
                   Console.WriteLine(e.Message);
                    this.isConnected = false;
                    return false;
                }
                
                return true;
            }
            return false;
        }

        /// <inheritdoc />
        public async Task<Message> GetPendingMessages()
        {
            Message receivedMessage;
            string messageData;


            receivedMessage = await deviceClient.ReceiveAsync().ConfigureAwait(false);

                if (receivedMessage != null)
                {

                    messageData = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    await deviceClient.CompleteAsync(receivedMessage).ConfigureAwait(false); 
                    Console.WriteLine(messageData);
                    return receivedMessage;
                }

            return null;

        }
    }
}
