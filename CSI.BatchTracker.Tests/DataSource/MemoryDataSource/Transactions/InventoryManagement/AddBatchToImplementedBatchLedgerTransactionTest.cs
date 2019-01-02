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
    class AddBatchToImplementedBatchLedgerTransactionTest
    {
        MemoryStore store;
        LoggedBatch loggedBatch;
        InventoryBatch inventoryBatch;
        BatchOperator batchOperator;
        AddBatchToImplementedBatchLedgerTransaction adder;
        AddReceivedBatchToInventoryTransaction receiver;
        [Test]
        public void ImplementingBatchRemovesBatchFromLiveInventory()
        {
            int expectedQty = 4;
            int expectedCount = 1;
            store = new MemoryStore();
            batchOperator = new BatchOperator("Jane", "Doe");
            inventoryBatch = new InventoryBatch("White", "8728811303201", DateTime.Now, 5);
            receiver = new AddReceivedBatchToInventoryTransaction(new Entity<InventoryBatch>(inventoryBatch), store);
            receiver.Execute();

            loggedBatch = new LoggedBatch(
                inventoryBatch.ColorName,
                inventoryBatch.BatchNumber,
                DateTime.Now,
                batchOperator
            );

            adder = new AddBatchToImplementedBatchLedgerTransaction(new Entity<LoggedBatch>(loggedBatch), store);
            adder.Execute();

            Assert.AreEqual(expectedQty, store.CurrentInventory[receiver.LastSystemId].NativeModel.Quantity);
            Assert.AreEqual(expectedCount, store.ImplementedBatchLedger.Count);
        }
    }
}
