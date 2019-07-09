using CSI.BatchTracker.Domain.DataSource.Contracts;
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
        public int PONumber { get; set; }
        public DateTime ReceivedDate { get; private set; }
        public BatchOperator ReceivingOperator { get; private set; }
        public ObservableCollection<ReceivedBatch> ReceivedBatches { get; private set; }
        public List<ReceivedBatch> RecordsToDelete { get; private set; }
        public List<ReceivedBatch> RecordsToUpdate { get; private set; }

        ReceivedPurchaseOrder originalPurchaseOrder;
        IActiveInventorySource inventorySource;
        IImplementedBatchSource implementedBatchSource;

        public ModifiedPurchaseOrderValidator(
            ObservableCollection<ReceivedBatch> receivedBatches,
            IActiveInventorySource inventorySource,
            IImplementedBatchSource implementedBatchSource)
        {
            if (receivedBatches.Count < 1)
            {
                string message = "Unable to instantiate with an empty set of received batches.";
                throw new ModifiedPurchaseOrderValidationException(message);
            }

            this.inventorySource = inventorySource;
            this.implementedBatchSource = implementedBatchSource;
            RecordsToDelete = new List<ReceivedBatch>();
            RecordsToUpdate = new List<ReceivedBatch>();
            ReceivedBatches = new ObservableCollection<ReceivedBatch>(receivedBatches);
            PONumber = ReceivedBatches[0].PONumber;
            ReceivedDate = ReceivedBatches[0].ActivityDate;
            ReceivingOperator = ReceivedBatches[0].ReceivingOperator;
            originalPurchaseOrder = new ReceivedPurchaseOrder(PONumber, ReceivedDate, ReceivingOperator, receivedBatches);
        }

        public void RunPurchaseOrderComparison()
        {
            CheckForUpdatedPONumber();
            CheckAndValidateDeletedRecords();
        }

        void CheckForUpdatedPONumber()
        {
            if (originalPurchaseOrder.PONumber != PONumber)
            {
                RecordsToUpdate.Clear();

                foreach (ReceivedBatch batch in ReceivedBatches)
                {
                    batch.PONumber = PONumber;
                    RecordsToUpdate.Add(batch);
                }
            }
        }

        void CheckAndValidateDeletedRecords()
        { 
            foreach (ReceivedBatch original in originalPurchaseOrder.ReceivedBatches)
            {
                if (BatchExistsInImplementationLedger(original))
                {
                    continue;
                }              

                if (RecordShouldBeDeleted(original))
                {
                    RecordsToDelete.Add(original);
                }
            }
        }

        bool RecordShouldBeDeleted(ReceivedBatch original)
        {
            foreach (ReceivedBatch batch in ReceivedBatches)
            {
                if (batch.BatchNumber == original.BatchNumber)
                {
                    return false;
                }
            }

            return true;
        }

        bool BatchExistsInImplementationLedger(ReceivedBatch batch)
        {
            ObservableCollection<LoggedBatch> batches = implementedBatchSource.GetImplementedBatchesByBatchNumber(batch.BatchNumber);
            return batches.Count > 0;
        }
    }
}
