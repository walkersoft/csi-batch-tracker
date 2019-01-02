using CSI.BatchTracker.Storage;
using CSI.BatchTracker.Storage.Contracts;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Storage.MemoryStore.Transactions.BatchOperators;
using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.DataSource.MemoryDataSource.Transactions.BatchOperators
{
    [TestFixture]
    class DeleteBatchOperatorAtIdTransactionTest
    {
        MemoryStore store;

        [SetUp]
        public void SetUp()
        {
            store = new MemoryStore();
        }

        [Test]
        public void ExistingBatchOperatorIsDeletedSuccessfully()
        {
            int targetId = 1;
            int expectedQty = 0;
            BatchOperator batchOperator = new BatchOperator("Jane", "Doe");
            Entity<BatchOperator> entity = new Entity<BatchOperator>(batchOperator);

            ITransaction adder = new AddBatchOperatorTransaction(entity, store);
            adder.Execute();

            ITransaction remover = new DeleteBatchOperatorAtIdTransaction(targetId, store);
            remover.Execute();

            Assert.AreEqual(expectedQty, store.BatchOperators.Count);
        }
    }
}
