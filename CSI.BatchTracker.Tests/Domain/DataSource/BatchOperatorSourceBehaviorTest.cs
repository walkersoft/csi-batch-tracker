using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Tests.TestHelpers.NativeModels;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.Domain.DataSource
{
    abstract class BatchOperatorSourceBehaviorTest
    {
        protected IBatchOperatorSource dataSource;
        BatchOperatorTestHelper helper;

        [SetUp]
        public virtual void SetUp()
        {
            helper = new BatchOperatorTestHelper();
        }

        [Test]
        public void SavingBatchOperatorResultsInRepositoryWithNewBatchOperatorAtLatestId()
        {
            int targetCollectionId = 0;
            BatchOperator batchOperator = helper.GetJaneDoeOperator();
            dataSource.SaveOperator(batchOperator);

            Assert.AreEqual(batchOperator.FirstName, dataSource.OperatorRepository[targetCollectionId].FirstName);
            Assert.AreEqual(batchOperator.FirstName, dataSource.OperatorRepository[targetCollectionId].FirstName);
        }

        [Test]
        public void SavingBatchOperatorAndRetrievingFromIdResultsInTheSameOperatorInfo()
        {
            int targetCollectionId = 0;
            BatchOperator batchOperator = helper.GetJaneDoeOperator();
            dataSource.SaveOperator(batchOperator);

            int targetId = dataSource.BatchOperatorIdMappings[targetCollectionId];
            BatchOperator found = dataSource.FindBatchOperator(targetId);

            Assert.AreEqual(batchOperator.FirstName, found.FirstName);
            Assert.AreEqual(batchOperator.LastName, found.LastName);
        }

        [Test]
        public void UpdatingBatchOperatorAtIdResultsInNewBatchOperatorInfoWhenLookedUp()
        {
            int targetCollectionId = 0;
            string expectedFirstName = "John";
            string expectedLastName = "Doe";
            BatchOperator batchOperator = helper.GetJaneDoeOperator();
            dataSource.SaveOperator(batchOperator);

            int targetId = dataSource.BatchOperatorIdMappings[targetCollectionId];
            dataSource.UpdateOperator(targetId, helper.GetJohnDoeOperator());
            BatchOperator found = dataSource.FindBatchOperator(targetId);

            Assert.AreEqual(expectedFirstName, found.FirstName);
            Assert.AreEqual(expectedLastName, found.LastName);
        }

        [Test]
        public void UpdatingBatchOperatorAtIdThatDoesNotExistResultsInNoChanges()
        {
            int targetCollectionId = 0;
            BatchOperator batchOperator = helper.GetJaneDoeOperator();
            dataSource.SaveOperator(batchOperator);
            int originalSize = dataSource.OperatorRepository.Count;

            int validId = dataSource.BatchOperatorIdMappings[targetCollectionId];
            dataSource.UpdateOperator(validId + 1, helper.GetJohnDoeOperator());

            Assert.AreEqual(originalSize, dataSource.OperatorRepository.Count);
        }

        [Test]
        public void DeletingBatchOperatorAtIdResultsInRepositoryThatIsOneLess()
        {
            int targetCollectionId = 0;
            int beforeDeleteCount = 1;
            int afterDeleteCount = beforeDeleteCount - 1;
            dataSource.SaveOperator(helper.GetJaneDoeOperator());
            Assert.AreEqual(beforeDeleteCount, dataSource.OperatorRepository.Count);

            int targetId = dataSource.BatchOperatorIdMappings[targetCollectionId];
            dataSource.DeleteBatchOperator(targetId);

            Assert.AreEqual(afterDeleteCount, dataSource.OperatorRepository.Count);
        }

        [Test]
        public void DeletingBatchOperatorAtIdThatDoesNotExistDoesNotChangeRepositorySize()
        {
            int expectedCount = 1;
            int invalidId = 100;
            dataSource.SaveOperator(helper.GetJaneDoeOperator());
            Assert.AreEqual(expectedCount, dataSource.OperatorRepository.Count);

            dataSource.DeleteBatchOperator(invalidId);

            Assert.AreEqual(expectedCount, dataSource.OperatorRepository.Count);
        }

        [Test]
        public void ListingBatchOperatorsResultsInRepositoryAndMappingsOfTheSameSize()
        {
            dataSource.SaveOperator(helper.GetJaneDoeOperator());
            dataSource.SaveOperator(helper.GetJaneDoeOperator());
            dataSource.FindAllBatchOperators();

            Assert.AreEqual(dataSource.OperatorRepository.Count, dataSource.BatchOperatorIdMappings.Count);
        }
    }
}
