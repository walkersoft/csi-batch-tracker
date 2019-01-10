using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Tests.TestHelpers.NativeModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.Domain.DataSource
{
    abstract class ReceivedBatchSourceBehaviorTest
    {
        protected IReceivedBatchSource dataSource;
        ReceivedBatchTestHelper helper;

        [SetUp]
        public virtual void SetUp()
        {
            helper = new ReceivedBatchTestHelper();
        }

        [Test]
        public void SavingReceivedBatchResultsInNewReceivedBatchAtLatestId()
        {
            int targetCollectionId = 0;
            ReceivedBatch batch = helper.GetUniqueBatch1();
            dataSource.SaveReceivedBatch(batch);

            ReceivedBatch stored = dataSource.ReceivedBatchRepository[targetCollectionId];
            AssertSameReceivedBatchData(batch, stored);
        }

        void AssertSameReceivedBatchData(ReceivedBatch expected, ReceivedBatch actual)
        {
            Assert.AreEqual(expected.ColorName, actual.ColorName);
            Assert.AreEqual(expected.BatchNumber, actual.BatchNumber);
            Assert.AreEqual(expected.ActivityDate, actual.ActivityDate);
            Assert.AreEqual(expected.Quantity, actual.Quantity);
            Assert.AreEqual(expected.PONumber, actual.PONumber);
            Assert.AreSame(expected.ReceivingOperator, actual.ReceivingOperator);
        }

        [Test]
        public void SavingReceivedBatchAndRetreivingFromIdResultsInTheSameBatchInfo()
        {
            int targetCollectionId = 0;
            ReceivedBatch batch = helper.GetUniqueBatch1();
            dataSource.SaveReceivedBatch(batch);

            int targetId = dataSource.ReceivedBatchIdMappings[targetCollectionId];
            ReceivedBatch found = dataSource.FindReceivedBatchById(targetId);

            AssertSameReceivedBatchData(batch, found);
        }

        [Test]
        public void UpdatingBatchOperatorAtIdResultsInNewBatchOperatorInfoWhenLookedUp()
        {
            int targetCollectionId = 0;
            ReceivedBatch original = helper.GetUniqueBatch1();
            dataSource.SaveReceivedBatch(original);

            int targetId = dataSource.ReceivedBatchIdMappings[targetCollectionId];
            ReceivedBatch updated = helper.GetUniqueBatch2();
            dataSource.UpdateReceivedBatch(targetId, updated);

            ReceivedBatch found = dataSource.FindReceivedBatchById(targetId);

            AssertSameReceivedBatchData(updated, found);
        }
    }
}
