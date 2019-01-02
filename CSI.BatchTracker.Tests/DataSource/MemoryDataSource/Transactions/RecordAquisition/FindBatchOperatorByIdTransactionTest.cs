using CSI.BatchTracker.Storage;
using CSI.BatchTracker.Storage.Contracts;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Storage.MemoryStore.Transactions.BatchOperators;
using CSI.BatchTracker.Storage.MemoryStore.Transactions.RecordAquisition;
using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.DataSource.MemoryDataSource.Transactions.RecordAquisition
{
    [TestFixture]
    class FindBatchOperatorByIdTransactionTest
    {
        MemoryStore store;

        [SetUp]
        public void SetUp()
        {
            store = new MemoryStore();
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
