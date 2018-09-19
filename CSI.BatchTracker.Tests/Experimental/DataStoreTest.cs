using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Experimental;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.Experimental
{
    [TestFixture]
    class DataStoreTest
    {
        DataStore store;
        ObservableCollection<BatchOperator> batchOperators;
        ObservableCollection<string> colors;
        ObservableCollection<ReceivedBatch> receivedBatches;
        ObservableCollection<LoggedBatch> loggedBatches;

        [SetUp]
        public void SetUp()
        {
            store = new DataStore();
            Random random = new Random();

            batchOperators = new ObservableCollection<BatchOperator>
            {
                new BatchOperator("Jane", "Doe"),
                new BatchOperator("John", "Doe"),
                new BatchOperator("Remy", "Quaid")
            };

            colors = new ObservableCollection<string>
            {
                "White", "Black", "Yellow", "Red", "Blue Red", "Bright Red", "Bright Yellow", "Deep Green", "Deep Blue"
            };

            receivedBatches = new ObservableCollection<ReceivedBatch>
            {
                new ReceivedBatch("White", "872881103201", DateTime.Parse("5/21/2018"), 6, 42018, batchOperators[random.Next(0, batchOperators.Count)]),
                new ReceivedBatch("White", "872881103201", DateTime.Parse("5/21/2018"), 2, 42018, batchOperators[random.Next(0, batchOperators.Count)]),
                new ReceivedBatch("Black", "872881503204", DateTime.Parse("5/21/2018"), 3, 42018, batchOperators[random.Next(0, batchOperators.Count)]),
                new ReceivedBatch("Yellow", "872880703401", DateTime.Parse("5/25/2018"), 5, 42018, batchOperators[random.Next(0, batchOperators.Count)]),
                new ReceivedBatch("White", "872881501703", DateTime.Parse("6/1/2018"), 8, 42033, batchOperators[random.Next(0, batchOperators.Count)]),
                new ReceivedBatch("Red", "872880404201", DateTime.Parse("6/1/2018"), 2, 42033, batchOperators[random.Next(0, batchOperators.Count)]),
                new ReceivedBatch("Blue Red", "872880901304", DateTime.Parse("6/1/2018"), 4, 42033, batchOperators[random.Next(0, batchOperators.Count)]),
                new ReceivedBatch("Bright Red", "872880101206", DateTime.Parse("6/1/2018"), 1, 42033, batchOperators[random.Next(0, batchOperators.Count)]),
                new ReceivedBatch("Deep Blue", "872881305103", DateTime.Parse("6/7/2018"), 1, 42084, batchOperators[random.Next(0, batchOperators.Count)]),
                new ReceivedBatch("Deep Green", "872880803205", DateTime.Parse("6/7/2018"), 3, 42084, batchOperators[random.Next(0, batchOperators.Count)]),
                new ReceivedBatch("Bright Yellow", "872880506701", DateTime.Parse("6/7/2018"), 2, 42084, batchOperators[random.Next(0, batchOperators.Count)]),
                new ReceivedBatch("Yellow", "872880703401", DateTime.Parse("6/7/2018"), 4, 42089, batchOperators[random.Next(0, batchOperators.Count)])
            };

            loggedBatches = new ObservableCollection<LoggedBatch>();
        }

        [Test]
        public void ColorNamesAreSetupAndEqualToNine()
        {
            store.Colors = colors;

            Assert.AreEqual(9, store.Colors.Count);
        }

        [Test]
        public void ReceivedBatchesAreSetupAndEqualToTwelve()
        {
            store.ReceivedBatches = receivedBatches;

            Assert.AreEqual(12, store.ReceivedBatches.Count);
        }

        [Test]
        public void InventoryBatchesAreSetupAndEqualToTen()
        {
            store.ReceivedBatches = receivedBatches;
            store.CalculateInventory();

            Assert.AreEqual(10, store.InventoryBatches.Count);

            int totalStock = 0;
            int expectedStock = 41;

            foreach (Entity<InventoryBatch> batch in store.InventoryBatches.Values)
            {
                totalStock += batch.NativeModel.Quantity;
            }

            Assert.AreEqual(expectedStock, totalStock);
        }

        [Test]
        public void InventoryBatchesMergedIntoSingleBatchNumber()
        {
            store.ReceivedBatches = receivedBatches;
            store.CalculateInventory();

            int expectedStock = 8;

            Assert.AreEqual(expectedStock, store.InventoryBatches[0].NativeModel.Quantity);
        }

        [Test]
        public void ImplementBatchReducesInventoryAndAddsToLedger()
        {
            store.ReceivedBatches = receivedBatches;
            store.CalculateInventory();

            store.ImplementBatch("872881103201", DateTime.Now, batchOperators[0]);

            Assert.AreEqual(1, store.LoggedBatches.Count);
            Assert.AreEqual(7, store.InventoryBatches[0].NativeModel.Quantity);
        }
    }
}
