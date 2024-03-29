using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ProcessDevice2DeviceMessages
{
    using System;
    using System.Configuration;
    using System.Text;
    using System.Threading.Tasks;
    using Client_Mobile.Enums;
    using Client_Mobile.Interfaces;
    using Client_Mobile.Models;
    using Client_Mobile.ServiceModels;
    using Microsoft.Azure.Devices;
    using Microsoft.Azure.Devices.Client;
    using Newtonsoft.Json;
    using Willezone.Azure.WebJobs.Extensions.DependencyInjection;
    using TransportType = Microsoft.Azure.Devices.Client.TransportType;

    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task Run([ServiceBusTrigger("d2dmessagesqueue",
                Connection = "MyServiceBusConnection")]
            string myQueueItem,
            ILogger log)
        {
            try
            {
                // create proxy 
                log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
                string connectionString = Constants.IotHubConnectionString;
                var client =
                    ServiceClient.CreateFromConnectionString(connectionString,
                        Microsoft.Azure.Devices.TransportType.Amqp);
                var receivedMessage = JsonConvert.DeserializeObject<GenericLockerRequest>(myQueueItem);
                var msgTo = receivedMessage.TargetedDeviceId;

                // send AMQP message
                var reSerializedMessage = JsonConvert.SerializeObject(receivedMessage);
                byte[] messageBytes = Encoding.UTF8.GetBytes(reSerializedMessage);
                var message = new Microsoft.Azure.Devices.Message(messageBytes);
                Console.WriteLine(message);
                await client.SendAsync(msgTo, message);
            }
            catch (FunctionException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}