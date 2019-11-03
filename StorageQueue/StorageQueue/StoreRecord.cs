using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using StorageQueue.Model;

namespace StorageQueue
{
    public static class StoreRecord
    {
        [FunctionName("StoreRecord")]
        public static void Run([QueueTrigger("LogMessageQueue")]string myQueueItem
            , [StorageAccount("AzureWebJobsStorage")]CloudStorageAccount storageAccount
            , ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            LogMessageQueue _logMessageQueue = new LogMessageQueue("LogMessageQueue", Guid.NewGuid().ToString());
            _logMessageQueue.Message = myQueueItem;
            _logMessageQueue.CreatedDate = DateTime.Now;

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("LogMessages");

            table.CreateIfNotExistsAsync().GetAwaiter().GetResult();

            TableOperation insertOperation = TableOperation.Insert(_logMessageQueue);

            table.ExecuteAsync(insertOperation).GetAwaiter().GetResult();

            log.LogInformation("Completed logging the message to the table");
        }
    }
}
