using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorageQueue.Model
{
    public class LogMessageQueue : TableEntity
    {
        public LogMessageQueue(string skey, string srow)
        {
            this.PartitionKey = skey;
            this.RowKey = srow;
        }

        public LogMessageQueue() { }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
