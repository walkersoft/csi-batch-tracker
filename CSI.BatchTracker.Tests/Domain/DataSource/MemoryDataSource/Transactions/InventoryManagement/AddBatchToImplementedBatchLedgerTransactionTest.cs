using CSI.BatchTracker.DataSource.MemoryDataSource;
using CSI.BatchTracker.DataSource.MemoryDataSource.Transactions.InventoryManagement;
using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.Domain.DataSource.MemoryDataSource.Transactions.InventoryManagement
{
    [TestFixture]
    class AddBatchToImplementedBatchLedgerTransactionTest
    {
        MemoryStore store;
        Entity<LoggedBatch> entity;
        LoggedBatch batch;
        BatchOperator batchOperator;
        AddBatchToImplementedBatchLedgerTransaction adder;

        [Test]
        public void AddImplementedBatchToLedger()
        {
            int expectedCount = 1;
            batchOperator = new BatchOperator("Jane", "Doe");
            batch = new LoggedBatch("White", "872881103201", DateTime.Now, batchOperator);
            entity = new Entity<LoggedBatch>(batch);
            adder = new AddBatchToImplementedBatchLedgerTransaction(entity, store);

            adder.Execute();

            Assert.AreEqual(expectedCount, store.ImplementedBatchLedger.Count);
        }
    }
}
