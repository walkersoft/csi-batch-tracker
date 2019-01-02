using CSI.BatchTracker.Storage;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Storage.MemoryStore.Transactions;
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
    class AddBatchOperatorTransactionTest
    {
        ITransaction adder;
        MemoryStore store;
        BatchOperator batchOperator;
        Entity<BatchOperator> entity;

        [SetUp]
        public void SetUp()
        {
            batchOperator = new BatchOperator("Jane", "Doe");
            entity = new Entity<BatchOperator>(batchOperator);
            store = new MemoryStore();
            adder = new AddBatchOperatorTransaction(entity, store);
        }

        [Test]
        public void AddingBatchOperatorResultsInNewRecordEntity()
        {
            int expectedCount = 1;            
            adder.Execute();

            Assert.AreEqual(expectedCount, store.BatchOperators.Count);         
        }

        [Test]
        public void AddingBatchOperatorProvidesNewSystemId()
        {
            int expectedSystemId = 1;
            adder.Execute();

            Assert.True(store.BatchOperators.ContainsKey(expectedSystemId));
        }

        [Test]
        public void AddingMultipleBatchOperatorsContinuesToIncrementSystemId()
        {
            int expectedSystemId = 2;

            adder.Execute();
            adder.Execute();

            Assert.True(store.BatchOperators.ContainsKey(expectedSystemId));
        }

        [Test]
        public void SystemIdRemainsSyncronizedAfterRemovingAnEntity()
        {
            int expectedSystemId = 3;
            int removedSystemId = 2;

            adder.Execute();
            adder.Execute();
            store.BatchOperators.Remove(removedSystemId);
            adder.Execute();

            Assert.True(store.BatchOperators.ContainsKey(expectedSystemId));
        }
    }
}
