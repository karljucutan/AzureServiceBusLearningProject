using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureServiceBusProccesor
{
    public class AzureServiceBusProcessor
    {
        private string _queueName;
        private string _connectionString;
        private ServiceBusClient _serviceBusClient;
        private ServiceBusProcessor _processor;

        public AzureServiceBusProcessor()
        {
            _queueName = "appqueue";
            _connectionString = "Endpoint=sb://kltjservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=2G9KfYhXCxg+BhcAew22nmbnJ514JjhE5+ASbOh32Ms=";
            _serviceBusClient = new ServiceBusClient(_connectionString);
            _processor = _serviceBusClient.CreateProcessor(_queueName,
                new ServiceBusProcessorOptions
                {
                    ReceiveMode = ServiceBusReceiveMode.PeekLock,
                    AutoCompleteMessages = false
                });

            // add handler to process messages
            _processor.ProcessMessageAsync += MessageHandler;

            // add handler to process any errors
            _processor.ProcessErrorAsync += ErrorHandler;
        }

        // handle received messages
        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            Console.WriteLine($"Received: {body}");

            // complete the message. message is deleted from the queue. 
            await args.CompleteMessageAsync(args.Message);
        }

        // handle any errors when receiving messages
        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        public async Task StartProcessingAsync()
        {
            // start processing 
            await _processor.StartProcessingAsync();

            Console.WriteLine("Wait for a minute and then press any key to end the processing");
            Console.ReadKey();
        }

        public async Task StopProcessingAsync()
        {
            // stop processing 
            Console.WriteLine("\nStopping the receiver...");
            await _processor.StopProcessingAsync();
            Console.WriteLine("Stopped receiving messages");

        }
    }
}
