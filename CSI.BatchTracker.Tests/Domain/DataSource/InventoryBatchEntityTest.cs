using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using NUnit.Framework;
using System;

namespace CSI.BatchTracker.Tests.Domain.DataSource
{
    [TestFixture]
    class InventoryBatchEntityTest
    {
        Entity<InventoryBatch> entity;
        readonly InventoryBatch batch = new InventoryBatch("Yellow", "872881501202", DateTime.Now, 1);

        [Test]
        public void SameNativeModelIsAvailable()
        {
            entity = new Entity<InventoryBatch>(batch);
            Assert.AreSame(batch, entity.NativeModel);
        }

        [Test]
        public void SystemIdIsZeroForNewEntity()
        {
            entity = new Entity<InventoryBatch>(batch);
            Assert.AreEqual(0, entity.SystemId);
        }

        [Test]
        public void CreateEntityWithExistingId()
        {
            int systemId = 4;
            entity = new Entity<InventoryBatch>(systemId, batch);
            Assert.AreEqual(systemId, entity.SystemId);
        }
    }
}
