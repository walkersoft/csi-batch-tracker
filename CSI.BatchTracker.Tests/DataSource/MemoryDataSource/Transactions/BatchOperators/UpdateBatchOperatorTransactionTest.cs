using CSI.BatchTracker.Storage;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Storage.MemoryStore.Transactions.BatchOperators;
using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.DataSource.MemoryDataSource.Transactions.BatchOperators
{
    [TestFixture]
    class UpdateBatchOperatorTransactionTest
    {
        [Test]
        public void UpdateExistingBatchOperatorEntityWithNewNativeModel()
        {
            MemoryStore store = new MemoryStore();
            BatchOperator before = new BatchOperator("Jane", "Doe");
            Entity<BatchOperator> entity = new Entity<BatchOperator>(before);

            ITransaction adder = new AddBatchOperatorTransaction(entity, store);
            adder.Execute();

            AddBatchOperatorTransaction added = adder as AddBatchOperatorTransaction;
            BatchOperator after = new BatchOperator("John", "Doe");
            entity = new Entity<BatchOperator>(added.LastSystemId, after);

            ITransaction updater = new UpdateBatchOperatorTransaction(entity, store);
            updater.Execute();

            ITransaction finder = new FindBatchOperatorTransaction(entity, store);
            finder.Execute();

            entity = finder.Results[0] as Entity<BatchOperator>;

            Assert.AreSame(after, entity.NativeModel);
        }
    }
}
