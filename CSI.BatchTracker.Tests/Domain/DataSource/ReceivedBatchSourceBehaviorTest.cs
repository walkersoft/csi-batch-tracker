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
            Assert.AreEqual(batch.ColorName, stored.ColorName);
            Assert.AreEqual(batch.BatchNumber, stored.BatchNumber);
            Assert.AreEqual(batch.ActivityDate, stored.ActivityDate);
            Assert.AreEqual(batch.Quantity, stored.Quantity);
            Assert.AreEqual(batch.PONumber, stored.PONumber);
            Assert.AreSame(batch.ReceivingOperator, stored.ReceivingOperator);
        }
    }
}
