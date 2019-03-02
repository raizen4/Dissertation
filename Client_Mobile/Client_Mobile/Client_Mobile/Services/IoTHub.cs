using System;
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

        public async Task<bool> CheckConnection()
        {
                try
                {
                   await _deviceClient.OpenAsync();                 
                   return true;
            }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
        }

        /// <inheritdoc />
        public async Task<bool> Lock(string targetedDeviceId, LockerActionEnum action)
        {
            if (!this._isConnected)
            {
                var recheckConn = await this.CheckConnection();
                if (recheckConn)
                {
                    this._isConnected = true;
                }
            }
            if (this._isConnected)
            {
                GenericLockerRequest req = new GenericLockerRequest();
                req.Action = action;
                req.TargetedDeviceId = targetedDeviceId;
                req.IotHubEndpoint = IotEndpointsEnum.D2DEndpoint;       
                var messageString = JsonConvert.SerializeObject(req);
                
                byte[] messageBytes = Encoding.UTF8.GetBytes(messageString);
                var message =new Message(messageBytes);
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

            return false;


        }

        /// <inheritdoc />
        public async Task<bool> Unlock(string targetedDeviceId, LockerActionEnum action)
        {
            if (this._isConnected)
            {
                if (!this._isConnected)
                {
                    var recheckConn = await this.CheckConnection();
                    if (recheckConn)
                    {
                        this._isConnected = true;
                    }
                }
                    GenericLockerRequest req = new GenericLockerRequest();
                    req.Action = action;
                    req.SenderDeviceId=
                    req.TargetedDeviceId = targetedDeviceId;
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

            return false;
        }

        /// <inheritdoc />
        
        /// <inheritdoc />
        public async Task<Message> GetPendingMessages()
        {
            Message receivedMessage;
            string messageData;


            receivedMessage = await _deviceClient.ReceiveAsync().ConfigureAwait(false);

                if (receivedMessage != null)
                {

                    messageData = Encoding.UTF8.GetString(receivedMessage.GetBytes());
                    await _deviceClient.CompleteAsync(receivedMessage).ConfigureAwait(false); 
                    Console.WriteLine(messageData);
                    return receivedMessage;
                }

            return null;

        }
    }
}
