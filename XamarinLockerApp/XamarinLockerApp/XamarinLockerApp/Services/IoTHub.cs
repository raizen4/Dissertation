﻿namespace XamarinLockerApp.Services
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Enums;
    using Interfaces;
    using Microsoft.Azure.Devices.Client;
    using Newtonsoft.Json;
    using ServiceModels;

    public class IoTHub : IIoTHub

    {

        private TransportType _transportProtocol;
        private string _connectionString;
        private static DeviceClient _deviceClient;


        public void InitializeConnectionToHub()
        {
            this._transportProtocol = TransportType.Http1; ;
            _deviceClient =
                DeviceClient.CreateFromConnectionString(Constants.UserLocker.IotHubConnectionString, this._transportProtocol);
        }
        
        /// <inheritdoc />
        public async Task<LockerMessage> GetPendingMessages()
        {
            Message receivedMessage;
            LockerMessage normalizedMessage = new LockerMessage();
            
            string messageData;

            if (_deviceClient == null)
            {
                InitializeConnectionToHub();
            }
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
            if (_deviceClient == null)
            {
                InitializeConnectionToHub();
            }
            lockerMessage.HasBeenSuccessful = true;
            var messageString = JsonConvert.SerializeObject(lockerMessage);
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
                return false;
            }

            return true;
        }

       
    }
}
