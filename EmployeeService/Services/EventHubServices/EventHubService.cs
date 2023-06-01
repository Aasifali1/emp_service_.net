using Azure.Identity;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Messaging.EventHubs.Producer;
using Azure.Storage.Blobs;
using System.Diagnostics;
using System.Text;

namespace EmployeeService.Services.EventHubServices
{
    public class EventHubService
    {
        // number of events to be sent to the event hub
        int numOfEvents = 3;

        // The Event Hubs client types are safe to cache and use as a singleton for the lifetime
        // of the application, which is best practice when events are being published or read regularly.
        // TODO: Replace the <EVENT_HUB_NAMESPACE> and <HUB_NAME> placeholder values
        //private readonly EventHubProducerClient producerClient;


        public EventHubService()
        {
/*            producerClient = new EventHubProducerClient(
            "EcommerceEventhub.servicebus.windows.net",
            "resourceaddedevent",
            new DefaultAzureCredential());*/
        }

        public async Task SendNotificationAsync(string message)
        {
            string connectionString = "Endpoint=sb://ecommerceeventhub.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=tcioxIuWtOktb60oQS0DL6T5q5yOQr0F3+AEhMnjtxI=";
            //EventDataBatch eventBatch = await producerClient.CreateBatchAsync();
            var eventHubName = "resourceaddedevent";

            await using (var producer = new EventHubProducerClient(connectionString, eventHubName))
            {
                using EventDataBatch eventBatch = await producer.CreateBatchAsync();
                eventBatch.TryAdd(new EventData(new BinaryData(message)));
                eventBatch.TryAdd(new EventData(new BinaryData(message)));

                await producer.SendAsync(eventBatch);
            }

/*            if (!eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(message))))
            {
                // if it is too large for the batch
                throw new Exception($"Event {message} is too large for the batch and cannot be sent.");
            }
            try
            {
                // Use the producer client to send the batch of events to the event hub
                await producerClient.SendAsync(eventBatch);
                Console.WriteLine($"A batch of events has been published.");
            }
            finally
            {
                await producerClient.DisposeAsync();
            }*/

        }

        public async Task ReciveEvents()
        {
            string blobstr = "DefaultEndpointsProtocol=https;AccountName=myecommerceapp;AccountKey=S4Jlr7p27GWgGEvWM/9iRbVk9Ch6UatVPcbIzqzI0i/EpGFqQSJO6nyooA70eZVtU0qD6HrrUTU4+ASthgRbFQ==;EndpointSuffix=core.windows.net";

            // Create a blob container client that the event processor will use 
            BlobContainerClient storageClient = new BlobContainerClient(
                blobstr, "productimages");

            string connectionString = "Endpoint=sb://ecommerceeventhub.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ii9bgAiPJW8mtUtkzuj2UEbDsiDIs82Xv+AEhKLfFN8=";

            try
            {
                var processor = new EventProcessorClient(
                storageClient,
                EventHubConsumerClient.DefaultConsumerGroupName,
                connectionString,
                "resourceaddedevent");

 /*               var processor = new EventProcessorClient(
                storageClient,
                EventHubConsumerClient.DefaultConsumerGroupName,
                "<EVENT_HUBS_NAMESPACE_CONNECTION_STRING>",
                "<HUB_NAME>");*/


                // Register handlers for processing events and handling errors
                processor.ProcessEventAsync += ProcessEventHandler;
                processor.ProcessErrorAsync += ProcessErrorHandler;

                // Start the processing
                await processor.StartProcessingAsync();
                Debug.WriteLine("hello");
                // Wait for 30 seconds for the events to be processed
                await Task.Delay(TimeSpan.FromSeconds(30));

                // Stop the processing
                await processor.StopProcessingAsync();
            }
            catch (Exception ex) { 
                throw new Exception(ex.Message, ex);
            }

        }

        async Task<String> ProcessEventHandler(ProcessEventArgs eventArgs)
        {
            // Write the body of the event to the console window
            Debug.WriteLine("\tReceived event: {0}", Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray()));
            Console.WriteLine("\tReceived event: {0}", Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray()));
            string result = Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray());
            return result;
        }

        Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
        {
            // Write details about the error to the console window
            Console.WriteLine($"\tPartition '{eventArgs.PartitionId}': an unhandled exception was encountered. This was not expected to happen.");
            Console.WriteLine(eventArgs.Exception.Message);
            return Task.CompletedTask;
        }
    }
}
