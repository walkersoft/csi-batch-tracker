using CSI.BatchTracker.DataSource;
using CSI.BatchTracker.DataSource.MemoryDataSource;
using CSI.BatchTracker.DataSource.MemoryDataSource.Transactions.RecordAquisition;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.DataSource.MemoryDataSource.Transactions.RecordAquisition
{
    [TestFixture]
    class ListReceivingLedgerTransactionTest
    {
        MemoryStore store;

        [Test]
        public void ListAllReceivedBatchesInReceivingLedger()
        {
            int expectedCount = 5;
            ITransaction finder = new ListReceivingLedgerTransaction(store);

            PopulateDataSourceWithFiveLedgerTransactions();
            finder.Execute();

            Assert.AreEqual(expectedCount, store);
        }

        void PopulateDataSourceWithFiveLedgerTransactions()
        {

        }
    }
}
