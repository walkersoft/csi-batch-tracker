using CSI.BatchTracker.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Domain.NativeModels
{
    public class ReceivedBatch : AbstractBatch
    {
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
            ActivityDate = receivingDate;
            Quantity = quantity;
            ReceivingOperator = receivingOperator;
        }
    }
}
