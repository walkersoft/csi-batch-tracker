using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using NUnit.Framework;
using System;

namespace CSI.BatchTracker.Tests.Domain.DataSource
{
    [TestFixture]
    class LoggedBatchEntityTest
    {
        Entity<LoggedBatch> entity;
        BatchOperator batchOperator = new BatchOperator("Jane", "Doe");
        LoggedBatch batch;

        [SetUp]
        public void SetUp()
        {
            batch = new LoggedBatch("Yellow", "872881203201", DateTime.Now, batchOperator);
        }

        [Test]
        public void SameNativeModelIsAvailable()
        {
            entity = new Entity<LoggedBatch>(batch);
            Assert.AreSame(batch, entity.NativeModel);
        }

        [Test]
        public void SystemIdIsZeroForNewEntity()
        {
            entity = new Entity<LoggedBatch>(batch);
            Assert.AreEqual(0, entity.SystemId);
        }

        [Test]
        public void CreateEntityWithExistingId()
        {
            int systemId = 4;
            entity = new Entity<LoggedBatch>(systemId, batch);
            Assert.AreEqual(systemId, entity.SystemId);
        }
    }
}
