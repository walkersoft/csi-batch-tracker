using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<ReceivedBatch> ReceivedBatches { get; private set; }

        Dictionary<int, int> batchSystemIdMappings;

        public EditablePurchaseOrder(
            ObservableCollection<ReceivedBatch> receivedBatches, 
            Dictionary<int, int> batchSystemIdMappings
        )
        {
            ReceivingDate = receivedBatches[0].ActivityDate;
            PONumber = receivedBatches[0].PONumber;
            ReceivingOperator = receivedBatches[0].ReceivingOperator;
            ReceivedBatches = receivedBatches;
            this.batchSystemIdMappings = batchSystemIdMappings;
        }

        public int GetReceivedBatchMappedSystemId(int collectionIndex)
        {
            return batchSystemIdMappings[collectionIndex];
        }
    }
}
