// See https://aka.ms/new-console-template for more information
using AzureServiceBusProccesor;

Console.WriteLine("Hello, World!");

AzureServiceBusProcessor azureServiceBusProcessor = new AzureServiceBusProcessor();

await azureServiceBusProcessor.StartProcessingAsync();

Console.ReadKey();