using CSI.BatchTracker.DataSource;
using CSI.BatchTracker.DataSource.MemoryDataSource;
using CSI.BatchTracker.DataSource.MemoryDataSource.Transactions;
using CSI.BatchTracker.DataSource.MemoryDataSource.Transactions.BatchOperators;
using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.Domain.DataSource.MemoryDataSource.Transactions.BatchOperators
{
    [TestFixture]
    class FindBatchOperatorTransactionTest
    {
        ITransaction adder;
        ITransaction finder;
        BatchOperator batchOperator;
        Entity<BatchOperator> entity;
        MemoryStore store;

        [SetUp]
        public void SetUp()
        {
            store = new MemoryStore();
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
