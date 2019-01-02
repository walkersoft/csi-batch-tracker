using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Storage.MemoryStore.Transactions.InventoryManagement;
using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.DataSource.MemoryDataSource.Transactions.InventoryManagement
{
    [TestFixture]
    class EditBatchInReceivingLedgerTransactionTest
    {
        MemoryStore store;
        Entity<ReceivedBatch> entity;
        AddReceivedBatchToReceivingLedgerTransaction adder;
        EditBatchInReceivingLedgerTransaction updater;
        string originalBatchNumber = "872881502102";
        string updatedBatchNumber = "872881805203";

        [Test]
        public void EditingBatchInLedgerChangesNativeModelInfo()
        {
            store = new MemoryStore();
            entity = GetOriginallyReceivedBatchEntity();

            adder = new AddReceivedBatchToReceivingLedgerTransaction(entity, store);
            adder.Execute();

            entity = GetUpdatedReceivedBatchEntity(adder.LastSystemId);
            ReceivedBatch expectedBatch = entity.NativeModel;

            updater = new EditBatchInReceivingLedgerTransaction(entity, store);
            updater.Execute();

            Assert.AreSame(expectedBatch, store.ReceivingLedger[adder.LastSystemId].NativeModel);
        }

        Entity<ReceivedBatch> GetOriginallyReceivedBatchEntity()
        {
            ReceivedBatch batch = new ReceivedBatch("White", originalBatchNumber, DateTime.Now, 5, 22222, GetBatchOperator());
            return new Entity<ReceivedBatch>(batch);
        }

        Entity<ReceivedBatch> GetUpdatedReceivedBatchEntity(int systemId)
        {
            ReceivedBatch batch = new ReceivedBatch("White", updatedBatchNumber, DateTime.Now, 5, 22222, GetBatchOperator());
            return new Entity<ReceivedBatch>(systemId, batch);
        }

        BatchOperator GetBatchOperator()
        {
            return new BatchOperator("Jane", "Doe");
        }
    }
}
