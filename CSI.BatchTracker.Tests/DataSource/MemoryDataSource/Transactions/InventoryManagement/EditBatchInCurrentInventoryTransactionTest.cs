using CSI.BatchTracker.DataSource.MemoryDataSource;
using CSI.BatchTracker.DataSource.MemoryDataSource.Transactions.InventoryManagement;
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
    class EditBatchInCurrentInventoryTransactionTest
    {
        MemoryStore store;
        Entity<InventoryBatch> entity;
        AddReceivedBatchToInventoryTransaction adder;
        EditBatchInCurrentInventoryTransaction updater;

        [Test]
        public void EditingBatchInCurrentInventoryChangesNativeModel()
        {
            MemoryStore store = new MemoryStore();
            adder = new AddReceivedBatchToInventoryTransaction(GetOriginalInventoryEntity(), store);
            adder.Execute();

            entity = GetUpdatedInventoryEntity(adder.LastSystemId);
            InventoryBatch expectedBatch = entity.NativeModel;

            updater = new EditBatchInCurrentInventoryTransaction(entity, store);
            updater.Execute();

            Assert.AreSame(expectedBatch, store.CurrentInventory[entity.SystemId].NativeModel);
        }

        Entity<InventoryBatch> GetOriginalInventoryEntity()
        {
            InventoryBatch batch = new InventoryBatch("White", "872882308203", DateTime.Now, 5);
            return new Entity<InventoryBatch>(batch);
        }

        Entity<InventoryBatch> GetUpdatedInventoryEntity(int systemId)
        {
            InventoryBatch batch = new InventoryBatch("White", "872882308203", DateTime.Now, 10);
            return new Entity<InventoryBatch>(systemId, batch);
        }

    }
}
