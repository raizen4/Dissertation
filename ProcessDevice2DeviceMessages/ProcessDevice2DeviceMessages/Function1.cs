using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ProcessDevice2DeviceMessages
{
    using System;
    using System.Configuration;
    using System.Text;
    using Client_Mobile.Enums;
    using Client_Mobile.Models;
    using Client_Mobile.ServiceModels;
    using Microsoft.Azure.Devices;
    using Microsoft.Azure.Devices.Client;
    using Newtonsoft.Json;

    using TransportType = Microsoft.Azure.Devices.Client.TransportType;

    public static class Function1
    {
        public static ServiceClient deviceClient;

        [FunctionName("Function1")]
        public static async void Run([ServiceBusTrigger("d2dmessagesqueue",  
            Connection ="MyServiceBusConnection")]
            string myQueueItem, 
            ILogger log)
        {
            try
            {
                var receivedMessage = JsonConvert.DeserializeObject<GenericLockerRequest>(myQueueItem);

                if (receivedMessage.Action == LockerActionEnum.Close ||
                    receivedMessage.Action == LockerActionEnum.Open)

                {   // create proxy 
                    log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
                    string connectionString = Constants.IotHubConnectionString;
                    var client = ServiceClient.CreateFromConnectionString(connectionString, Microsoft.Azure.Devices.TransportType.Amqp);
                    var msgTo = receivedMessage.TargetedDeviceId;
                    var reSerializedMessage = JsonConvert.SerializeObject(receivedMessage);
                    byte[] messageBytes = Encoding.UTF8.GetBytes(reSerializedMessage);
                    var message = new Microsoft.Azure.Devices.Message(messageBytes);
                    Console.WriteLine(message);

                    // send AMQP message
                    await client.SendAsync(msgTo, message);
                }
                else
                {
                    if (receivedMessage.Action == LockerActionEnum.Closed ||
                        receivedMessage.Action == LockerActionEnum.Opened)
                    {

                    }
                }
               
              
            }
            catch (FunctionException e)
            {
                Console.WriteLine(e.Message);
            }
          
        }
    }
}
