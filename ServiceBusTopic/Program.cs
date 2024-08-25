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