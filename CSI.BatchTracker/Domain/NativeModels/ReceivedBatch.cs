using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Domain.NativeModels
{
    public class ReceivedBatch
    {
        public string ColorName { get; private set; }
        public string BatchNumber { get; private set; }
        public DateTime ReceivingDate { get; private set; }
        public int Quantity { get; private set; }
        public BatchOperator ReceivingOperator { get; private set; }

        public ReceivedBatch(
            string colorName, 
            string batchNumber, 
            DateTime receivingDate, 
            int quantity, 
            BatchOperator receivingOperator
        )
        {
            ColorName = colorName;
            BatchNumber = batchNumber;
            ReceivingDate = receivingDate;
            Quantity = quantity;
            ReceivingOperator = receivingOperator;
        }
    }
}
