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
    class FindBatchOperatorByIdTransactionTest
    {
        MemoryStoreContext store;

        [SetUp]
        public void SetUp()
        {
            store = new MemoryStoreContext();
        }

        [Test]
        public void NonExistantSystemIdProducesNoResults()
        {
            int targetId = 1;
            int expectedQty = 0;

            ITransaction finder = new FindBatchOperatorByIdTransaction(targetId, store);

            Assert.AreEqual(expectedQty, finder.Results.Count);
        }

        [Test]
        public void FindBatchOperatorWithExistingSystemId()
        {
            int targetId = 1;
            int expectedQty = 1;
            PutSingleEntryInBatchOperatorsDataSource();

            ITransaction finder = new FindBatchOperatorByIdTransaction(targetId, store);
            finder.Execute();

            Assert.AreEqual(expectedQty, finder.Results.Count);
        }

        void PutSingleEntryInBatchOperatorsDataSource()
        {
            BatchOperator batchOperator = new BatchOperator("Jane", "Doe");
            Entity<BatchOperator> entity = new Entity<BatchOperator>(batchOperator);
            ITransaction adder = new AddBatchOperatorTransaction(entity, store);

            adder.Execute();
        }
    }
}
