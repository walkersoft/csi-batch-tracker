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
    class AddReceivedBatchToReceivingLedgerTransactionTest
    {
        MemoryStoreContext store;
        Entity<ReceivedBatch> entity;
        AddReceivedBatchToReceivingLedgerTransaction adder;

        [Test]
        public void AddingReceivedBatchToLedgerIncreasesEntryCount()
        {
            int expectedCount = 1;
            int expectedSystemId = 1;
            store = new MemoryStoreContext();
            ReceivedBatch received = new ReceivedBatch("White", "872881201308", DateTime.Now, 5, 22392, new BatchOperator("Jane", "Doe"));
            entity = new Entity<ReceivedBatch>(received);

            adder = new AddReceivedBatchToReceivingLedgerTransaction(entity, store);
            adder.Execute();

            Assert.AreEqual(expectedCount, store.ReceivingLedger.Count);
            Assert.AreEqual(expectedSystemId, adder.LastSystemId);
        }
    }
}
