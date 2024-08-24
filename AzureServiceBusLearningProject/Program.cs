using Azure.Messaging.ServiceBus;
using System.Text;
using System.Text.Json;

namespace AzureServiceBusLearningProject
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var azureServiceBus = new AzureServiceBus();

            List<Order> orders = new List<Order>
            {
                new Order { Id = 1, CourseName = "AZ-204 Developer", Price = 10.99m },
                new Order { Id = 2, CourseName = "AZ-104 Azure Admin", Price = 11.99m },
                new Order { Id = 3, CourseName = "DP-203 Azure Data Engineer", Price = 12.99m }
            };

            await azureServiceBus.SendMessageAsyc(orders);

            //await azureServiceBus.PeekMessagesAsync(10);

            Console.WriteLine("\n\n\n Received messages");
            //await azureServiceBus.ReceiveMessageAsync(10);
        }
    }

    public class AzureServiceBus
    {
        private string _queueName;
        private string _connectionString;
        private ServiceBusClient _serviceBusClient;

        public AzureServiceBus()
        {
            _queueName = "appqueue";
            _connectionString = "Endpoint=sb://kltjservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=2G9KfYhXCxg+BhcAew22nmbnJ514JjhE5+ASbOh32Ms=";
            _serviceBusClient = new ServiceBusClient(_connectionString);
        }

        public async Task SendMessageAsyc(List<Order> orders)
        {
            var serviceBusSender = _serviceBusClient.CreateSender(_queueName);

            using ServiceBusMessageBatch serviceBusMessageBatch = await serviceBusSender.CreateMessageBatchAsync();
            int id = 0;
            foreach(var order in orders)
            {
                ServiceBusMessage serviceBusMessage =
                    new ServiceBusMessage(JsonSerializer.Serialize(order));
                serviceBusMessage.ContentType = "application/json";
                serviceBusMessage.ApplicationProperties.Add("Month", "August");

                // set message id for duplicate detection, it wont send a message to a queue for a specific time with the same id.
                serviceBusMessage.MessageId = id.ToString(); id ++;

                serviceBusMessageBatch.TryAddMessage(serviceBusMessage);
            }

            await serviceBusSender.SendMessagesAsync(serviceBusMessageBatch);

            Console.WriteLine($"Message sent");
        }

        public async Task PeekMessagesAsync(int maxMessageCount)
        {
            var serviceBusReceiver = _serviceBusClient.CreateReceiver(_queueName);

            var peekMessages = await serviceBusReceiver.PeekMessagesAsync(maxMessageCount);

            foreach(var message in peekMessages)
            {
                Console.WriteLine($"Message Id {message.MessageId}");
                Console.WriteLine($"Message Body {message.Body}");
                Console.WriteLine($"Message Properties {message.ApplicationProperties["Month"]}");
            }
        }

        public async Task ReceiveMessageAsync(int maxMessageCount)
        {
            var serviceBusReceiver = _serviceBusClient.CreateReceiver(
                _queueName,
                new ServiceBusReceiverOptions 
                {
                    ReceiveMode = ServiceBusReceiveMode.PeekLock
                });

            var receivedMessages = await serviceBusReceiver.ReceiveMessagesAsync(maxMessageCount);

            foreach (var message in receivedMessages)
            {
                Console.WriteLine($"Message Id {message.MessageId}");
                Console.WriteLine($"Message Body {message.Body}");
                // Complete the message after processing
                await serviceBusReceiver.CompleteMessageAsync(message);
            }
        }

        public async Task GetQueueLength()
        {

        }
    }


    public class Order
    {
        public int Id { get; set; }
        public string? CourseName { get; set; }
        public decimal Price { get; set; }
    }
}
