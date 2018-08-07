using System;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Contracts;
using CSI.BatchTracker.Domain.DataSource;
using NUnit.Framework;
using CSI.BatchTracker.Experimental;
using CSI.BatchTracker.Domain.DataSource.Repositories;

namespace CSI.BatchTracker.Tests.Domain
{
    [TestFixture]
    class DataSourceTest
    {
        IDataSource data;

        [SetUp]
        public void SetUp()
        {
            data = new DataSourceRepository(new DataStore());
        }

        [Test]
        public void AddBatchToInventory()
        {
            int expectedCount = 1;
            int expectedQty = 5;
            ReceivedBatch batch = GetReceiveableWhiteBaseBatch();

            data.ReceiveInventory(batch);

            Assert.AreEqual(expectedCount, data.InventoryRepository.Count);
            Assert.AreEqual(expectedQty, data.InventoryRepository[0].Quantity);
        }

        [Test]
        public void AddOperatorToRepository()
        {
            int expectedCount = 1;
            BatchOperator batchOperator = GetJaneDoeOperator();

            data.SaveOperator(batchOperator);

            Assert.AreEqual(expectedCount, data.OperatorRepository.Items.Count);
            Assert.AreSame(batchOperator, data.OperatorRepository.Items[0].NativeModel);
        }

        [Test]
        public void AddInventoryBatchToLedger()
        {
            int expectedInventoryStock = 4;
            int expectedLedgerCount = 1;
            ReceivedBatch received = GetReceiveableWhiteBaseBatch();

            data.ReceiveInventory(received);
            data.ImplementBatch("872871701203", DateTime.Now, GetJaneDoeOperator());

            Assert.AreEqual(expectedInventoryStock, data.InventoryRepository[0].Quantity); //want this to be InventoryRepository.GetStockStatus(string batchNumber)
            Assert.AreEqual(expectedLedgerCount, data.BatchLedger.Count);
        }

        [Test]
        public void DepletedBatchIsRemovedFromInventoryRepo()
        {
            int expectedStock = 0;
            int expectedLedgerCount = 5;
            ReceivedBatch received = GetReceiveableWhiteBaseBatch();

            //receiving will add 5 to inventory - need to implement all five afterwards.
            data.ReceiveInventory(received);
            data.ImplementBatch("872871701203", DateTime.Now, GetJaneDoeOperator());
            data.ImplementBatch("872871701203", DateTime.Now, GetJaneDoeOperator());
            data.ImplementBatch("872871701203", DateTime.Now, GetJaneDoeOperator());
            data.ImplementBatch("872871701203", DateTime.Now, GetJaneDoeOperator());
            data.ImplementBatch("872871701203", DateTime.Now, GetJaneDoeOperator());

            Assert.AreEqual(expectedStock, data.InventoryRepository.Count);
            Assert.AreEqual(expectedLedgerCount, data.BatchLedger.Count);
        }

        [Test]
        public void RemoveCorrectBatchFromInventoryRepo()
        {
            int expectedStock = 1;

            data.ReceiveInventory(GetReceiveableWhiteBaseBatch());
            data.ReceiveInventory(GetReceiveableBlackBaseBatch());
            data.ImplementBatch("872882501302", DateTime.Now, GetJaneDoeOperator());

            Assert.AreEqual(expectedStock, data.InventoryRepository[1].Quantity);
        }

        ReceivedBatch GetReceiveableWhiteBaseBatch()
        {
            string color = "White";
            string batch = "872871701203";
            DateTime date = DateTime.Now;
            int qty = 5;
            int poNumber = 41202;
            BatchOperator batchOperator = GetJaneDoeOperator();

            return new ReceivedBatch(color, batch, date, qty, poNumber, batchOperator);
        }

        ReceivedBatch GetReceiveableBlackBaseBatch()
        {
            string color = "Black";
            string batch = "872882501302";
            DateTime date = DateTime.Now;
            int qty = 2;
            int poNumber = 42195;
            BatchOperator batchOperator = GetJaneDoeOperator();

            return new ReceivedBatch(color, batch, date, qty, poNumber, batchOperator);
        }

        BatchOperator GetJaneDoeOperator()
        {
            return new BatchOperator("Jane", "Doe");
        }
    }
}
