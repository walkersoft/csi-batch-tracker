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

namespace CSI.BatchTracker.Tests.Storage.MemoryStore.Transactions.InventoryManagement
{
    [TestFixture]
    class UndoImplementedBatchCommittedToLedgerTransactionTest
    {
        MemoryStoreContext store;
        Entity<LoggedBatch> loggedEntity;
        Entity<InventoryBatch> inventoryEntity;
        UndoImplementedBatchCommittedToLedgerTransaction undo;
        AddReceivedBatchToInventoryTransaction inventoryAdder;
        AddBatchToImplementedBatchLedgerTransaction inventoryImplementer;

        [SetUp]
        public void SetUp()
        {
            store = new MemoryStoreContext();
        }

        [Test]
        public void UndoingBatchRemovesExistingEntryFromImplementationLedger()
        {
            int expectedCountBefore = 1;
            int expectedCountAfter = 0;
            int targetId = AddBatchToInventoryThenLedger();
            
            Assert.AreEqual(expectedCountBefore, store.ImplementedBatchLedger.Count);

            undo = new UndoImplementedBatchCommittedToLedgerTransaction(store.ImplementedBatchLedger[targetId], store);
            undo.Execute();

            Assert.AreEqual(expectedCountAfter, store.ImplementedBatchLedger.Count);
        }
        
        [Test]
        public void UndoingBatchRecommitsBatchToInventoryAndAddsEntryIfBatchDoesNotExist()
        {
            int expectedCount = 1;
            int expectedQty = 1;
            int targetId = AddBatchToInventoryThenLedger();

            undo = new UndoImplementedBatchCommittedToLedgerTransaction(store.ImplementedBatchLedger[targetId], store);
            undo.Execute();

            Assert.AreEqual(expectedCount, store.CurrentInventory.Count);
            Assert.AreEqual(expectedQty, store.CurrentInventory[inventoryAdder.LastSystemId].NativeModel.Quantity);
        }

        [Test]
        public void UndoingBatchRecommitsBatchToInventoryAndMergesWithExistingBatch()
        {
            int expectedQtyBefore = 2;
            int expectedQtyAfter = 3;
            int expectedCount = 0;
            int targetId = AddThreeOfSameBatchToInventoryThenOneToLedger();

            Assert.AreEqual(expectedQtyBefore, store.CurrentInventory[inventoryAdder.LastSystemId].NativeModel.Quantity);

            undo = new UndoImplementedBatchCommittedToLedgerTransaction(store.ImplementedBatchLedger[targetId], store);
            undo.Execute();

            Assert.AreEqual(expectedQtyAfter, store.CurrentInventory[inventoryAdder.LastSystemId].NativeModel.Quantity);
            Assert.AreEqual(expectedCount, store.ImplementedBatchLedger.Count);
        }

        int AddBatchToInventoryThenLedger()
        {
            AddBatchToInventory();

            loggedEntity = GetLoggedBatchEntityFromInventoryBatchEntity(store.CurrentInventory[inventoryAdder.LastSystemId]);

            inventoryImplementer = new AddBatchToImplementedBatchLedgerTransaction(loggedEntity, store);
            inventoryImplementer.Execute();

            return inventoryImplementer.LastSystemId;
        }

        int AddThreeOfSameBatchToInventoryThenOneToLedger()
        {
            InventoryBatch batch = new InventoryBatch("White", "872882210102", DateTime.Now, 3);
            inventoryEntity = new Entity<InventoryBatch>(batch);
            inventoryAdder = new AddReceivedBatchToInventoryTransaction(inventoryEntity, store);
            inventoryAdder.Execute();

            loggedEntity = GetLoggedBatchEntityFromInventoryBatchEntity(inventoryEntity);
            inventoryImplementer = new AddBatchToImplementedBatchLedgerTransaction(loggedEntity, store);
            inventoryImplementer.Execute();

            return inventoryImplementer.LastSystemId;
        }

        void AddBatchToInventory()
        {
            inventoryEntity = GetInventoryBatchEntity();

            inventoryAdder = new AddReceivedBatchToInventoryTransaction(inventoryEntity, store);
            inventoryAdder.Execute();
        }

        Entity<InventoryBatch> GetInventoryBatchEntity()
        {
            InventoryBatch batch = new InventoryBatch("White", "872882210102", DateTime.Now, 1);
            return new Entity<InventoryBatch>(batch);
        }

        Entity<LoggedBatch> GetLoggedBatchEntityFromInventoryBatchEntity(Entity<InventoryBatch> entity)
        {
            LoggedBatch logged = new LoggedBatch(
                entity.NativeModel.ColorName,
                entity.NativeModel.BatchNumber,
                DateTime.Now,
                new BatchOperator("Jane", "Doe")
            );

            return new Entity<LoggedBatch>(logged);
        }

    }
}
