using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.Domain.DataSource
{
    [TestFixture]
    class ReceivedBatchEntityTest
    {
        Entity<ReceivedBatch> entity;
        readonly BatchOperator batchOperator = new BatchOperator("Jane", "Doe");
        ReceivedBatch batch;

        [SetUp]
        public void SetUp()
        {
            batch = new ReceivedBatch("Yellow", "872881505205", DateTime.Now, 1, 44444, batchOperator);
        }

        [Test]
        public void SameNativeModelIsAvailable()
        {
            entity = new Entity<ReceivedBatch>(batch);
            Assert.AreSame(batch, entity.NativeModel);
        }

        [Test]
        public void SystemIdIsZeroForNewEntity()
        {
            entity = new Entity<ReceivedBatch>(batch);
            Assert.AreEqual(0, entity.SystemId);
        }

        [Test]
        public void CreateEntityWithExistingId()
        {
            int systemId = 4;
            entity = new Entity<ReceivedBatch>(systemId, batch);
            Assert.AreEqual(systemId, entity.SystemId);
        }
    }
}
