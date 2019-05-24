using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Tests.TestHelpers.NativeModels;
using NUnit.Framework;
using System;
using System.Collections.ObjectModel;

namespace CSI.BatchTracker.Tests.Domain.DataSource.Behaviors
{
    abstract class ReceivedBatchSourceBehaviorTest
    {
        protected IReceivedBatchSource receivedBatchSource;
        protected IActiveInventorySource inventorySource;
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
            receivedBatchSource.SaveReceivedBatch(batch);

            ReceivedBatch stored = receivedBatchSource.ReceivedBatchRepository[targetCollectionId];
            AssertSameReceivedBatchData(batch, stored);
        }

        [Test]
        public void SavingReceivedBatchAlsoUpdatesActiveInventoryWithNewRecord()
        {
            ReceivedBatch batch = helper.GetUniqueBatch1();
            string batchNumber = batch.BatchNumber;

            receivedBatchSource.SaveReceivedBatch(batch);
            InventoryBatch stored = inventorySource.FindInventoryBatchByBatchNumber(batchNumber);

            Assert.AreEqual(batchNumber, stored.BatchNumber);
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
            receivedBatchSource.SaveReceivedBatch(batch);

            int targetId = receivedBatchSource.ReceivedBatchIdMappings[targetCollectionId];
            ReceivedBatch found = receivedBatchSource.FindReceivedBatchById(targetId);

            AssertSameReceivedBatchData(batch, found);
        }

        [Test]
        public void UpdatingReceivedBatchAtIdResultsInNewBatchOperatorInfoWhenLookedUp()
        {
            int targetCollectionId = 0;
            ReceivedBatch original = helper.GetUniqueBatch1();
            receivedBatchSource.SaveReceivedBatch(original);

            int targetId = receivedBatchSource.ReceivedBatchIdMappings[targetCollectionId];
            ReceivedBatch updated = helper.GetUniqueBatch2();
            receivedBatchSource.UpdateReceivedBatch(targetId, updated);

            ReceivedBatch found = receivedBatchSource.FindReceivedBatchById(targetId);

            AssertSameReceivedBatchData(updated, found);
        }

        [Test]
        public void UpdatingReceivedBatchAtIdThatDoesNotExistResultsInNoChanges()
        {
            int targetCollectionId = 0;
            ReceivedBatch batch = helper.GetUniqueBatch1();
            receivedBatchSource.SaveReceivedBatch(batch);
            int originalSize = receivedBatchSource.ReceivedBatchRepository.Count;

            int targetId = receivedBatchSource.ReceivedBatchIdMappings[targetCollectionId];
            ReceivedBatch updated = helper.GetUniqueBatch2();
            receivedBatchSource.UpdateReceivedBatch(targetId + 1, updated);

            Assert.AreEqual(originalSize, receivedBatchSource.ReceivedBatchRepository.Count);
        }

        [Test]
        public void DeletingReceivedBatchAtIdResultsInRepositoryThatIsOneLess()
        {
            int targetCollectionId = 0;
            int beforeDeleteCount = 1;
            int afterDeleteCount = beforeDeleteCount - 1;
            receivedBatchSource.SaveReceivedBatch(helper.GetUniqueBatch1());
            Assert.AreEqual(beforeDeleteCount, receivedBatchSource.ReceivedBatchRepository.Count);

            int targetId = receivedBatchSource.ReceivedBatchIdMappings[targetCollectionId];
            receivedBatchSource.DeleteReceivedBatch(targetId);

            Assert.AreEqual(afterDeleteCount, receivedBatchSource.ReceivedBatchRepository.Count);
        }

        [Test]
        public void DeletingReceivedBatchAtIdThatDoesNotExistDoesNotChangeRepositorySize()
        {
            int expectedCount = 1;
            int invalidId = 100;
            receivedBatchSource.SaveReceivedBatch(helper.GetUniqueBatch1());

            receivedBatchSource.DeleteReceivedBatch(invalidId);

            Assert.AreEqual(expectedCount, receivedBatchSource.ReceivedBatchRepository.Count);
        }

        [Test]
        public void ListingReceivedBatchesResultsInRepositoryAndMappingsOfTheSameSize()
        {
            receivedBatchSource.SaveReceivedBatch(helper.GetUniqueBatch1());
            receivedBatchSource.SaveReceivedBatch(helper.GetUniqueBatch2());
            receivedBatchSource.FindAllReceivedBatches();

            Assert.AreEqual(receivedBatchSource.ReceivedBatchRepository.Count, receivedBatchSource.ReceivedBatchIdMappings.Count);
        }

        [Test]
        public void FindingReceivedBatchesByPONumberExcludesBatchesThatDoNotMeetCriteria()
        {
            int expectedCount = 2;
            int poNumberCriteria = 11111;
            receivedBatchSource.SaveReceivedBatch(helper.GetUniqueBatch1());
            receivedBatchSource.SaveReceivedBatch(helper.GetBatchWithSpecificPO(poNumberCriteria));
            receivedBatchSource.SaveReceivedBatch(helper.GetBatchWithSpecificPO(poNumberCriteria));

            receivedBatchSource.FindReceivedBatchesByPONumber(poNumberCriteria);

            Assert.AreEqual(expectedCount, receivedBatchSource.ReceivedBatchRepository.Count);
        }

        [Test]
        public void FindingReceivedBatchesByReceivingDateExcludesBatchesThatDoNotMeetCriteria()
        {
            int expectedCount = 2;
            DateTime dateCriteria = DateTime.Now.AddDays(1);
            receivedBatchSource.SaveReceivedBatch(helper.GetUniqueBatch1());
            receivedBatchSource.SaveReceivedBatch(helper.GetBatchWithSpecificDate(dateCriteria));
            receivedBatchSource.SaveReceivedBatch(helper.GetBatchWithSpecificDate(dateCriteria));

            receivedBatchSource.FindReceivedBatchesByDate(dateCriteria);

            Assert.AreEqual(expectedCount, receivedBatchSource.ReceivedBatchRepository.Count);
        }

        [Test]
        public void FindingReceivedBatchByReceivingDateOnlyConsidersSameMonthDayAndYear()
        {
            int expectedCount = 2;
            DateTime dateCriteria = new DateTime(2019, 1, 14, 0, 0, 0);
            DateTime dateCriteria1 = dateCriteria.AddHours(6);
            DateTime dateCriteria2 = dateCriteria.AddHours(10);
            DateTime dateCriteria3 = dateCriteria.AddDays(1);
            receivedBatchSource.SaveReceivedBatch(helper.GetBatchWithSpecificDate(dateCriteria1));
            receivedBatchSource.SaveReceivedBatch(helper.GetBatchWithSpecificDate(dateCriteria2));
            receivedBatchSource.SaveReceivedBatch(helper.GetBatchWithSpecificDate(dateCriteria3));

            receivedBatchSource.FindReceivedBatchesByDate(dateCriteria);

            Assert.AreEqual(expectedCount, receivedBatchSource.ReceivedBatchRepository.Count);
        }

        [Test]
        public void FindingAllBatchesBySpecificBatchNumber()
        {
            int expectedCount = 2;
            ReceivedBatch targetBatch = helper.GetUniqueBatch1();
            ReceivedBatch paddingBatch = helper.GetUniqueBatch2();

            for (int i = 0; i < 5; i++)
            {
                targetBatch.ActivityDate = DateTime.Today.AddDays(i);
                targetBatch.Quantity = i;
                targetBatch.PONumber = targetBatch.PONumber + i;

                if (i % 2 == 0)
                {
                    receivedBatchSource.SaveReceivedBatch(paddingBatch);
                }
                else
                {
                    receivedBatchSource.SaveReceivedBatch(targetBatch);
                }
            }

            ObservableCollection<ReceivedBatch> batches;
            batches = receivedBatchSource.GetReceivedBatchesByBatchNumber(targetBatch.BatchNumber);

            Assert.AreEqual(expectedCount, batches.Count);
        }
    }
}
