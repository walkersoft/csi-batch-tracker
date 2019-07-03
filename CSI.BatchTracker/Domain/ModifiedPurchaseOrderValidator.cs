using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Domain
{
    public sealed class ModifiedPurchaseOrderValidator
    {
        public int PONumber { get; private set; }
        public DateTime ReceivedDate { get; private set; }
        public BatchOperator ReceivingOperator { get; private set; }
        public ObservableCollection<ReceivedBatch> ReceivedBatches { get; private set; }

        ReceivedPurchaseOrder originalPurchaseOrder;

        public ModifiedPurchaseOrderValidator(ObservableCollection<ReceivedBatch> receivedBatches)
        {
            if (receivedBatches.Count < 1)
            {
                string message = "Unable to instantiate with an empty set of received batches.";
                throw new ModifiedPurchaseOrderValidationException(message);
            }

            ReceivedBatches = receivedBatches;
            InitializeOriginalPurchaseOrder();
        }

        void InitializeOriginalPurchaseOrder()
        {
            PONumber = ReceivedBatches[0].PONumber;
            ReceivedDate = ReceivedBatches[0].ActivityDate;
            ReceivingOperator = ReceivedBatches[0].ReceivingOperator;
        }
    }
}
