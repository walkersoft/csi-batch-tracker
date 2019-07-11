using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Domain
{
    public sealed class EditablePurchaseOrder
    {
        public DateTime ReceivingDate { get; set; }
        public int PONumber { get; set; }
        public BatchOperator ReceivingOperator { get; set; }
        public List<ReceivedBatch> ReceivedBatches { get; private set; }

        public EditablePurchaseOrder(List<ReceivedBatch> receivedBatches)
        {
            ReceivingDate = receivedBatches[0].ActivityDate;
            PONumber = receivedBatches[0].PONumber;
            ReceivingOperator = receivedBatches[0].ReceivingOperator;
            ReceivedBatches = receivedBatches;
        }
    }
}
