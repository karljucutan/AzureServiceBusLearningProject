// See https://aka.ms/new-console-template for more information
using ServiceBusTopic;

Console.WriteLine("Hello, World!");



List<Order> orders = new List<Order>
            {
                new Order { Id = 1, CourseName = "AZ-204 Developer", Price = 10.99m },
                new Order { Id = 2, CourseName = "AZ-104 Azure Admin", Price = 11.99m },
                new Order { Id = 3, CourseName = "DP-203 Azure Data Engineer", Price = 12.99m }
            };


AzureServiceBus azureServiceBus = new AzureServiceBus();

await azureServiceBus.SendMessageAsyc(orders);

//await azureServiceBus.PeekMessagesAsync(3);

//await azureServiceBus.SubscriptionAReceiveMessageAsync(3);


// In Azure Service Bus Dashboard.
// Created a 1isnot0false filter and deleted sa default filter.
// SQL filter is "1=0" resulting to SubscriptionA to not received the messages.