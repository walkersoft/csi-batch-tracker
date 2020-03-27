using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.DataSource.SQLiteStore;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Storage.SQLiteStore;
using CSI.BatchTracker.Tests.TestHelpers.NativeModels;
using CSI.BatchTracker.Tests.TestHelpers.Storage.SQLiteStore;
using CSI.BatchTracker.ViewModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CSI.BatchTracker.Tests.ViewModels.WithSQLiteStore
{
    [TestFixture]
    class AverageBatchBehaviorTest
    {
        IBatchOperatorSource operatorSource;
        IActiveInventorySource inventorySource;
        IReceivedBatchSource receivedBatchSource;
        IImplementedBatchSource implementedBatchSource;
        MainWindowViewModel viewModel;
        BatchOperatorTestHelper operatorHelper;
        SQLiteStoreContext sqliteStore;
        SQLiteDatabaseHelper dbHelper;
        BatchOperator batchOperator;

        [SetUp]
        public void SetUp()
        {
            dbHelper = new SQLiteDatabaseHelper();
            dbHelper.CreateTestDatabase();
            sqliteStore = new SQLiteStoreContext(dbHelper.DatabaseFile);
            operatorSource = new SQLiteBatchOperatorSource(sqliteStore);
            inventorySource = new SQLiteActiveInventorySource(sqliteStore);
            receivedBatchSource = new SQLiteReceivedBatchSource(sqliteStore, inventorySource);
            implementedBatchSource = new SQLiteImplementedBatchSource(sqliteStore, inventorySource);
            operatorHelper = new BatchOperatorTestHelper(operatorSource);
            viewModel = new MainWindowViewModel(inventorySource, receivedBatchSource, implementedBatchSource, operatorSource);
            batchOperator = operatorHelper.GetJaneDoeOperator();
        }

        [Test]
        public void ReceiveAndImplementBatchesForThirtyDaysAndGetAverages()
        {
            DateTime dateRangeEnd = DateTime.Now;
            DateTime dateRangeStart = dateRangeEnd.AddDays(-30);
            List<ReceivedBatch> receivedBatches = GetReceivedBatches();
            List<float> expectedUsage = new List<float> { 0.66f, 0.46f, 0.4f, 0.33f, 0.26f, 0.2f, 0.16f, 0.13f, 0.1f };

            foreach (ReceivedBatch batch in receivedBatches)
            {
                batch.ActivityDate = dateRangeStart;
                receivedBatchSource.SaveReceivedBatch(batch);
                bool dateToggle = true;

                while (batch.Quantity > 0)
                {
                    implementedBatchSource.AddBatchToImplementationLedger(batch.BatchNumber, dateToggle ? dateRangeStart : dateRangeEnd, batchOperator);
                    batch.Quantity--;
                    dateToggle = !dateToggle;
                }
            }

            viewModel.AssociateCollectionsAndRepositories();

            for (int i = 0; i < viewModel.AverageBatchList.Count; i++)
            {
                Assert.AreEqual(expectedUsage[i], viewModel.AverageBatchList[i].AverageUsage, .01f);
            }
        }

        [Test]
        public void ReceiveAndImplementBatchesForTwentyDays()
        {
            DateTime dateRangeEnd = DateTime.Now;
            DateTime dateRangeStart = dateRangeEnd.AddDays(-20);
            List<ReceivedBatch> receivedBatches = GetReceivedBatches();
            List<float> expectedUsage = new List<float> { 1.0f, 0.7f, 0.6f, 0.5f, 0.4f, 0.3f, 0.25f, 0.2f, 0.15f };

            foreach (ReceivedBatch batch in receivedBatches)
            {
                batch.ActivityDate = dateRangeStart;
                receivedBatchSource.SaveReceivedBatch(batch);
                bool dateToggle = true;

                while (batch.Quantity > 0)
                {
                    implementedBatchSource.AddBatchToImplementationLedger(batch.BatchNumber, dateToggle ? dateRangeStart : dateRangeEnd, batchOperator);
                    batch.Quantity--;
                    dateToggle = !dateToggle;
                }
            }

            viewModel.AssociateCollectionsAndRepositories();

            for (int i = 0; i < viewModel.AverageBatchList.Count; i++)
            {
                Assert.AreEqual(expectedUsage[i], viewModel.AverageBatchList[i].AverageUsage, .01f);
            }
        }

        List<ReceivedBatch> GetReceivedBatches()
        {
            return new List<ReceivedBatch>()
            {
                new ReceivedBatch("White", "872890111111", DateTime.Now, 20, 11111, batchOperator),
                new ReceivedBatch("Black", "872890122222", DateTime.Now, 12, 11111, batchOperator),
                new ReceivedBatch("Yellow", "872890133333", DateTime.Now, 10, 11111, batchOperator),
                new ReceivedBatch("Red", "872890144444", DateTime.Now, 4, 11111, batchOperator),
                new ReceivedBatch("Blue Red", "872890155555", DateTime.Now, 14, 11111, batchOperator),
                new ReceivedBatch("Deep Green", "872890166666", DateTime.Now, 6, 11111, batchOperator),
                new ReceivedBatch("Deep Blue", "872890177777", DateTime.Now, 8, 11111, batchOperator),
                new ReceivedBatch("Bright Red", "872890188888", DateTime.Now, 5, 11111, batchOperator),
                new ReceivedBatch("Bright Yellow", "872890199999", DateTime.Now, 3, 11111, batchOperator)
            };
        }
    }
}
