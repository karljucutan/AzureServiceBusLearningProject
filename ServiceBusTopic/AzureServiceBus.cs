using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServiceBusTopic
{
    public class AzureServiceBus
    {
        private string _topicName;
        private string _connectionString;
        private ServiceBusClient _serviceBusClient;

        public AzureServiceBus()
        {
            _topicName = "apptopic";
            _connectionString = "Endpoint=sb://kltjservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=oF9p9x0HGcCQTI+YbVK5zMEH/z8Up/+eY+ASbPEV/8Q=";
            _serviceBusClient = new ServiceBusClient(_connectionString);
        }

        public async Task SendMessageAsyc(List<Order> orders)
        {
            var serviceBusSender = _serviceBusClient.CreateSender(_topicName);

            using ServiceBusMessageBatch serviceBusMessageBatch = await serviceBusSender.CreateMessageBatchAsync();
            int id = 0;
            foreach (var order in orders)
            {
                ServiceBusMessage serviceBusMessage =
                    new ServiceBusMessage(JsonSerializer.Serialize(order));
                serviceBusMessage.ContentType = "application/json";
                serviceBusMessage.ApplicationProperties.Add("Month", "August");

                // set message id for duplicate detection, it wont send a message to a queue for a specific time with the same id.
                //serviceBusMessage.MessageId = id.ToString(); id++;

                serviceBusMessageBatch.TryAddMessage(serviceBusMessage);
            }

            await serviceBusSender.SendMessagesAsync(serviceBusMessageBatch);

            Console.WriteLine($"Message sent");
        }

    }

    public class Order
    {
        public int Id { get; set; }
        public string? CourseName { get; set; }
        public decimal Price { get; set; }
    }
}
