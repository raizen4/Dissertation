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
        private  DeviceClient _deviceClient;
       


        public  bool  InitializeIotHub()
        {
            try
            {
            this._transportProtocol = TransportType.Http1; ;
            this._deviceClient =
                DeviceClient.CreateFromConnectionString(Constants.IotHubConnectionString, this._transportProtocol);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> CheckConnection()
        {
                try
                {
                   await this._deviceClient.OpenAsync();                 
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
            if (this._deviceClient == null)
            {
               var initialized= InitializeIotHub();
                if (!initialized)
                    return false;
                
            }
                LockerMessage req = new LockerMessage();
                Console.WriteLine(Constants.IotHubConnectionString);
                req.Action = action;
                req.SenderDeviceId = Constants.CurrentLoggedInUser.DeviceId;
                req.TargetedDeviceId = targetedDeviceId;
                req.IotHubEndpoint = IotEndpointsEnum.D2DEndpoint;       
                var messageString = JsonConvert.SerializeObject(req);
                
                byte[] messageBytes = Encoding.UTF8.GetBytes(messageString);
                var message =new Message(messageBytes);
                message.Properties.Add("IotHubEndpoint", IotEndpointsEnum.D2DEndpoint);          
                try
                {
                    await this._deviceClient.SendEventAsync(message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                 
                    return false;
                }

                return true;
            



        }

        /// <inheritdoc />
        public async Task<bool> Unlock(string targetedDeviceId, LockerActionEnum action)
        {


            if (this._deviceClient == null)
            {
                var initialized = InitializeIotHub();
                if (!initialized)
                    return false;

            }

            LockerMessage req = new LockerMessage();
                    req.Action = action;
                    req.SenderDeviceId = Constants.CurrentLoggedInUser.DeviceId;
                    req.TargetedDeviceId = targetedDeviceId;
                    req.IotHubEndpoint = IotEndpointsEnum.D2DEndpoint;
                    var messageString = JsonConvert.SerializeObject(req);

                    byte[] messageBytes = Encoding.UTF8.GetBytes(messageString);
                    var message = new Message(messageBytes);
                    message.Properties.Add("IotHubEndpoint", IotEndpointsEnum.D2DEndpoint);
                    try
                {
                    await this._deviceClient.SendEventAsync(message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);  
                    return false;
                }

                return true;
            

          
        }

        /// <inheritdoc />
        
        /// <inheritdoc />
        public async Task<LockerMessage> GetPendingMessages()
        {
            Message receivedMessage;
            LockerMessage normalizedMessage = new LockerMessage();
            string messageData;

            if (this._deviceClient == null)
            {
                var initialized = InitializeIotHub();
                if (!initialized)
                    return null;

            }

            receivedMessage = await this._deviceClient.ReceiveAsync().ConfigureAwait(false);

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
    }
}
