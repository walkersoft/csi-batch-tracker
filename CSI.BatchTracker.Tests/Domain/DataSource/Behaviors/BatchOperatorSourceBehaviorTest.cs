using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Tests.TestHelpers.NativeModels;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.Domain.DataSource.Behaviors
{
    abstract class BatchOperatorSourceBehaviorTest
    {
        protected IBatchOperatorSource operatorSource;
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
            operatorSource.SaveOperator(batchOperator);

            Assert.AreEqual(batchOperator.FirstName, operatorSource.OperatorRepository[targetCollectionId].FirstName);
            Assert.AreEqual(batchOperator.FirstName, operatorSource.OperatorRepository[targetCollectionId].FirstName);
        }

        [Test]
        public void SavingBatchOperatorAndRetrievingFromIdResultsInTheSameOperatorInfo()
        {
            int targetCollectionId = 0;
            BatchOperator batchOperator = helper.GetJaneDoeOperator();
            operatorSource.SaveOperator(batchOperator);

            int targetId = operatorSource.BatchOperatorIdMappings[targetCollectionId];
            BatchOperator found = operatorSource.FindBatchOperator(targetId);

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
            operatorSource.SaveOperator(batchOperator);

            int targetId = operatorSource.BatchOperatorIdMappings[targetCollectionId];
            operatorSource.UpdateOperator(targetId, helper.GetJohnDoeOperator());
            BatchOperator found = operatorSource.FindBatchOperator(targetId);

            Assert.AreEqual(expectedFirstName, found.FirstName);
            Assert.AreEqual(expectedLastName, found.LastName);
        }

        [Test]
        public void UpdatingBatchOperatorAtIdThatDoesNotExistResultsInNoChanges()
        {
            int targetCollectionId = 0;
            BatchOperator batchOperator = helper.GetJaneDoeOperator();
            operatorSource.SaveOperator(batchOperator);
            int originalSize = operatorSource.OperatorRepository.Count;

            int validId = operatorSource.BatchOperatorIdMappings[targetCollectionId];
            operatorSource.UpdateOperator(validId + 1, helper.GetJohnDoeOperator());

            Assert.AreEqual(originalSize, operatorSource.OperatorRepository.Count);
        }

        [Test]
        public void DeletingBatchOperatorAtIdResultsInRepositoryThatIsOneLess()
        {
            int targetCollectionId = 0;
            int beforeDeleteCount = 1;
            int afterDeleteCount = beforeDeleteCount - 1;
            operatorSource.SaveOperator(helper.GetJaneDoeOperator());
            Assert.AreEqual(beforeDeleteCount, operatorSource.OperatorRepository.Count);

            int targetId = operatorSource.BatchOperatorIdMappings[targetCollectionId];
            operatorSource.DeleteBatchOperator(targetId);

            Assert.AreEqual(afterDeleteCount, operatorSource.OperatorRepository.Count);
        }

        [Test]
        public void DeletingBatchOperatorAtIdThatDoesNotExistDoesNotChangeRepositorySize()
        {
            int expectedCount = 1;
            int invalidId = 100;
            operatorSource.SaveOperator(helper.GetJaneDoeOperator());
            Assert.AreEqual(expectedCount, operatorSource.OperatorRepository.Count);

            operatorSource.DeleteBatchOperator(invalidId);

            Assert.AreEqual(expectedCount, operatorSource.OperatorRepository.Count);
        }

        [Test]
        public void ListingBatchOperatorsResultsInRepositoryAndMappingsOfTheSameSize()
        {
            operatorSource.SaveOperator(helper.GetJaneDoeOperator());
            operatorSource.SaveOperator(helper.GetJaneDoeOperator());
            operatorSource.FindAllBatchOperators();

            Assert.AreEqual(operatorSource.OperatorRepository.Count, operatorSource.BatchOperatorIdMappings.Count);
        }
    }
}
