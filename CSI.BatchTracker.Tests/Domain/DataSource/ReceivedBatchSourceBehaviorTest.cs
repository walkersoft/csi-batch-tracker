using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Tests.TestHelpers.NativeModels;
using NUnit.Framework;
using System;

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
        public void UpdatingReceivedBatchAtIdResultsInNewBatchOperatorInfoWhenLookedUp()
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

        [Test]
        public void UpdatingReceivedBatchAtIdThatDoesNotExistResultsInNoChanges()
        {
            int targetCollectionId = 0;
            ReceivedBatch batch = helper.GetUniqueBatch1();
            dataSource.SaveReceivedBatch(batch);
            int originalSize = dataSource.ReceivedBatchRepository.Count;

            int targetId = dataSource.ReceivedBatchIdMappings[targetCollectionId];
            ReceivedBatch updated = helper.GetUniqueBatch2();
            dataSource.UpdateReceivedBatch(targetId + 1, updated);

            Assert.AreEqual(originalSize, dataSource.ReceivedBatchRepository.Count);
        }

        [Test]
        public void DeletingReceivedBatchAtIdResultsInRepositoryThatIsOneLess()
        {
            int targetCollectionId = 0;
            int beforeDeleteCount = 1;
            int afterDeleteCount = beforeDeleteCount - 1;
            dataSource.SaveReceivedBatch(helper.GetUniqueBatch1());
            Assert.AreEqual(beforeDeleteCount, dataSource.ReceivedBatchRepository.Count);

            int targetId = dataSource.ReceivedBatchIdMappings[targetCollectionId];
            dataSource.DeleteReceivedBatch(targetId);

            Assert.AreEqual(afterDeleteCount, dataSource.ReceivedBatchRepository.Count);
        }

        [Test]
        public void DeletingReceivedBatchAtIdThatDoesNotExistDoesNotChangeRepositorySize()
        {
            int expectedCount = 1;
            int invalidId = 100;
            dataSource.SaveReceivedBatch(helper.GetUniqueBatch1());

            dataSource.DeleteReceivedBatch(invalidId);

            Assert.AreEqual(expectedCount, dataSource.ReceivedBatchRepository.Count);
        }

        [Test]
        public void ListingReceivedBatchesResultsInRepositoryAndMappingsOfTheSameSize()
        {
            dataSource.SaveReceivedBatch(helper.GetUniqueBatch1());
            dataSource.SaveReceivedBatch(helper.GetUniqueBatch2());
            dataSource.FindAllReceivedBatches();

            Assert.AreEqual(dataSource.ReceivedBatchRepository.Count, dataSource.ReceivedBatchIdMappings.Count);
        }

        [Test]
        public void FindingReceivedBatchesByPONumberExcludesBatchesThatDoNotMeetCriteria()
        {
            int expectedCount = 2;
            int poNumberCriteria = 11111;
            dataSource.SaveReceivedBatch(helper.GetUniqueBatch1());
            dataSource.SaveReceivedBatch(helper.GetBatchWithSpecificPO(poNumberCriteria));
            dataSource.SaveReceivedBatch(helper.GetBatchWithSpecificPO(poNumberCriteria));

            dataSource.FindReceivedBatchesByPONumber(poNumberCriteria);

            Assert.AreEqual(expectedCount, dataSource.ReceivedBatchRepository.Count);
        }

        [Test]
        public void FindingReceivedBatchesByReceivingDateExcludesBatchesThatDoNotMeetCriteria()
        {
            int expectedCount = 2;
            DateTime dateCriteria = DateTime.Now.AddDays(1);
            dataSource.SaveReceivedBatch(helper.GetUniqueBatch1());
            dataSource.SaveReceivedBatch(helper.GetBatchWithSpecificDate(dateCriteria));
            dataSource.SaveReceivedBatch(helper.GetBatchWithSpecificDate(dateCriteria));

            dataSource.FindReceivedBatchesByDate(dateCriteria);

            Assert.AreEqual(expectedCount, dataSource.ReceivedBatchRepository.Count);
        }

        [Test]
        public void FindingReceivedBatchByReceivingDateOnlyConsidersSameMonthDayAndYear()
        {
            int expectedCount = 2;
            DateTime dateCriteria = new DateTime(2019, 1, 14, 0, 0, 0);
            DateTime dateCriteria1 = dateCriteria.AddHours(6);
            DateTime dateCriteria2 = dateCriteria.AddHours(10);
            DateTime dateCriteria3 = dateCriteria.AddDays(1);
            dataSource.SaveReceivedBatch(helper.GetBatchWithSpecificDate(dateCriteria1));
            dataSource.SaveReceivedBatch(helper.GetBatchWithSpecificDate(dateCriteria2));
            dataSource.SaveReceivedBatch(helper.GetBatchWithSpecificDate(dateCriteria3));

            dataSource.FindReceivedBatchesByDate(dateCriteria);

            Assert.AreEqual(expectedCount, dataSource.ReceivedBatchRepository.Count);
        }
    }
}
