using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Storage.Contracts;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Storage.MemoryStore.Transactions.BatchOperators;
using CSI.BatchTracker.Storage.MemoryStore.Transactions.RecordAquisition;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.Storage.MemoryStore.Transactions.RecordAquisition
{
    [TestFixture]
    class ListBatchOperatorsTransactionTest
    {
        MemoryStoreContext store;

        [SetUp]
        public void SetUp()
        {
            store = new MemoryStoreContext();
        }

        [Test]
        public void GetListOfBatchOperators()
        {
            int expectedCount = 3;
            ITransaction finder = new ListBatchOperatorsTransaction(store);

            PopulateDataSourceWithThreeBatchOperators();
            finder.Execute();

            Assert.AreEqual(expectedCount, finder.Results.Count);
        }

        void PopulateDataSourceWithThreeBatchOperators()
        {
            Entity<BatchOperator> entity = new Entity<BatchOperator>(new BatchOperator("Jane", "Doe"));
            ITransaction adder = new AddBatchOperatorTransaction(entity, store);
            adder.Execute();
            adder.Execute();
            adder.Execute();
        }
    }
}
