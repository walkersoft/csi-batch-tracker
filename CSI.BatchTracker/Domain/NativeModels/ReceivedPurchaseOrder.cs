using System;
using System.Collections.ObjectModel;

namespace CSI.BatchTracker.Domain.NativeModels
{
    public class ReceivedPurchaseOrder
    {
        public int PONumber { get; private set; }
        public DateTime ActivityDate { get; private set; }
        public BatchOperator ReceivingOperator { get; private set; }
        public ObservableCollection<ReceivedBatch> ReceivedBatches { get; private set; }

        public string DisplayDate
        {
            get
            {
                return ActivityDate.ToString("MMMM d, yyyy");
            }
        }

        public ReceivedPurchaseOrder(int poNumber, DateTime activityDate, BatchOperator receivingOperator, ObservableCollection<ReceivedBatch> receivedBatches)
        {
            PONumber = poNumber;
            ActivityDate = activityDate;
            ReceivingOperator = receivingOperator;
            ReceivedBatches = receivedBatches;
        }

        public ReceivedPurchaseOrder(int poNumber, DateTime activityDate, BatchOperator receivingOperator)
        {
            PONumber = poNumber;
            ActivityDate = activityDate;
            ReceivingOperator = receivingOperator;
            ReceivedBatches = new ObservableCollection<ReceivedBatch>();
        }

        public void AddBatch(ReceivedBatch received)
        {
            ReceivedBatches.Add(received);
        }
    }
}
