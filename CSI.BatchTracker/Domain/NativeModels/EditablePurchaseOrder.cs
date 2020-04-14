using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CSI.BatchTracker.Domain.NativeModels
{
    public sealed class EditablePurchaseOrder
    {
        public DateTime ReceivingDate { get; set; }
        public int PONumber { get; set; }
        public BatchOperator ReceivingOperator { get; set; }
        public ObservableCollection<ReceivedBatch> ReceivedBatches { get; private set; }

        Dictionary<int, int> batchSystemIdMappings;

        public EditablePurchaseOrder(
            ObservableCollection<ReceivedBatch> receivedBatches,
            Dictionary<int, int> batchSystemIdMappings
        )
        {
            if (receivedBatches.Count > 0)
            {
                ReceivingDate = receivedBatches[0].ActivityDate;
                PONumber = receivedBatches[0].PONumber;
                ReceivingOperator = receivedBatches[0].ReceivingOperator;
            }

            ReceivedBatches = receivedBatches;
            this.batchSystemIdMappings = batchSystemIdMappings;
        }

        public int GetReceivedBatchMappedSystemId(int collectionIndex)
        {
            return batchSystemIdMappings[collectionIndex];
        }
    }
}
