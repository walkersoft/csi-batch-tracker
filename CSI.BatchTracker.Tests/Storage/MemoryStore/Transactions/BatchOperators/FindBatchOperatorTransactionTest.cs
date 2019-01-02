using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Storage.Contracts;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Storage.MemoryStore.Transactions.BatchOperators;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.Storage.MemoryStore.Transactions.BatchOperators
{
    [TestFixture]
    class FindBatchOperatorTransactionTest
    {
        ITransaction adder;
        ITransaction finder;
        BatchOperator batchOperator;
        Entity<BatchOperator> entity;
        MemoryStoreContext store;

        [SetUp]
        public void SetUp()
        {
            store = new MemoryStoreContext();
            batchOperator = new BatchOperator("Jane", "Doe");
            entity = new Entity<BatchOperator>(batchOperator);
        }

        [Test]
        public void NoBatchOperatorEntitiesFoundIfStoreIsEmpty()
        {
            int expectedCount = 0;

            finder = new FindBatchOperatorTransaction(entity, store);
            finder.Execute();

            Assert.AreEqual(expectedCount, finder.Results.Count);
        }

        [Test]
        public void FindBatchOperatorEntityBySystemId()
        {
            int systemId = 1;
            int expectedCount = 1;

            adder = new AddBatchOperatorTransaction(entity, store);
            adder.Execute();
            entity = new Entity<BatchOperator>(systemId, entity.NativeModel);
            finder = new FindBatchOperatorTransaction(entity, store);
            finder.Execute();

            Assert.AreEqual(expectedCount, finder.Results.Count);
        }
    }
}
