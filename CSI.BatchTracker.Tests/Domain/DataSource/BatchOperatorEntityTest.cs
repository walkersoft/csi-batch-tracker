using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.Domain.DataSource
{
    [TestFixture]
    class BatchOperatorEntityTest
    {
        Entity<BatchOperator> entity;
        readonly BatchOperator batchOperator = new BatchOperator("Jane", "Doe");

        [Test]
        public void SameNativeModelIsAvailable()
        {
            entity = new Entity<BatchOperator>(batchOperator);
            Assert.AreSame(batchOperator, entity.NativeModel);
        }

        [Test]
        public void SystemIdIsZeroForNewEntity()
        {
            entity = new Entity<BatchOperator>(batchOperator);
            Assert.AreEqual(0, entity.SystemId);
        }

        [Test]
        public void CreateEntityWithExistingId()
        {
            int systemId = 4;
            entity = new Entity<BatchOperator>(systemId, batchOperator);
        }
    }
}
