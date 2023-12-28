using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumiaAzureServiceBus
{
    public class MessageBus : IMessageBus
    {
        private readonly string _connstring = "Endpoint=sb://jumiams.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=38D5qVQ/sQxXKD4XBl4A47KpOtCcf1aLi+ASbKiYxHI=";
        public async Task PublishMessage(object message, string Topic_or_Queue)
        {
            // Create a client 

            ServiceBusClient Client = new ServiceBusClient(_connstring);

            ServiceBusSender sender = Client.CreateSender(Topic_or_Queue);

            // Process message to be sent 

            var messagebody = JsonConvert.SerializeObject(message);

            ServiceBusMessage finalmessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(messagebody));

            // Send Message

            await sender.SendMessageAsync(finalmessage);

            // Free up used resources

            await sender.DisposeAsync();

        }
    }
}
