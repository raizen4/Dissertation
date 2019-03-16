﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LockerApp.Services
{
    using System.Threading;
    using System.Threading.Tasks;
    using Enums;
    using Interfaces;
    using Microsoft.Azure.Devices.Client;
    using Models;
    using Newtonsoft.Json;
    using ServiceModels;


    public class IoTHub : IIoTHub

    {

        private TransportType _transportProtocol;
        private string _connectionString;
        private static DeviceClient _deviceClient;
        private bool _isConnected = false;

        /// <inheritdoc />
        ///
        ///
        ///
        ///
        ///

        public IoTHub()
        {
            this._transportProtocol = TransportType.Http1; ;
            _deviceClient =
                    DeviceClient.CreateFromConnectionString(Constants.IotHubConnectionString, this._transportProtocol);
        
        }

       
        
        /// <inheritdoc />
        public async Task<LockerMessage> GetPendingMessages()
        {
            Message receivedMessage;
            LockerMessage normalizedMessage = new LockerMessage();
            
            string messageData;


            receivedMessage = await _deviceClient.ReceiveAsync().ConfigureAwait(false);

                if (receivedMessage != null)
                {

                    messageData = Encoding.UTF8.GetString(receivedMessage.GetBytes());
                    normalizedMessage = JsonConvert.DeserializeObject<LockerMessage>(messageData);
                    await _deviceClient.CompleteAsync(receivedMessage).ConfigureAwait(false); 
                    Console.WriteLine(messageData);
                    return normalizedMessage;
                }

            return null;

        }

      
        public async Task<bool> SendMessageToLocker(LockerMessage lockerMessage)
        {
            LockerMessage req = new LockerMessage();
            req.ActionRequest = lockerMessage.ActionRequest;
            req.TargetedDeviceId = lockerMessage.TargetedDeviceId;
            req.IotHubEndpoint = IotEndpointsEnum.D2DEndpoint;
            var messageString = JsonConvert.SerializeObject(req);

            byte[] messageBytes = Encoding.UTF8.GetBytes(messageString);
            var message = new Message(messageBytes);
            message.Properties.Add("IotHubEndpoint", IotEndpointsEnum.D2DEndpoint);
            try
            {
                await _deviceClient.SendEventAsync(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                this._isConnected = false;
                return false;
            }

            return true;
        }

       
    }
}
