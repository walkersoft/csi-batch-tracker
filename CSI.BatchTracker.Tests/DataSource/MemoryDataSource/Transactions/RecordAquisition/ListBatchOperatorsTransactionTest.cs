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
    class ListBatchOperatorsTransactionTest
    {
        MemoryStore store;

        [SetUp]
        public void SetUp()
        {
            store = new MemoryStore();
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
