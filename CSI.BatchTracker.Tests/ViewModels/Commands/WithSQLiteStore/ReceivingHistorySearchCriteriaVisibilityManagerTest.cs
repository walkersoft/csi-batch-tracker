﻿using CSI.BatchTracker.Domain;
using CSI.BatchTracker.Domain.DataSource.SQLiteStore;
using CSI.BatchTracker.Storage.SQLiteStore;
using CSI.BatchTracker.Tests.TestHelpers.Storage.SQLiteStore;
using CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors;
using CSI.BatchTracker.ViewModels;
using CSI.BatchTracker.ViewModels.Commands;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.WithSQLiteStore
{
    [TestFixture]
    class ReceivingHistorySearchCriteriaVisibilityManagerTest : ReceivingHistorySearchCriteriaVisibilityManagerCommandBehaviorTest
    {
        SQLiteDatabaseHelper sqliteHelper;

        [SetUp]
        public void SetUp()
        {
            sqliteHelper = new SQLiteDatabaseHelper();
            sqliteHelper.CreateTestDatabase();
            SQLiteStoreContext context = new SQLiteStoreContext(sqliteHelper.DatabaseFile);
            inventorySource = new SQLiteActiveInventorySource(context);
            receivedBatchSource = new SQLiteReceivedBatchSource(context, inventorySource);
            implementedBatchSource = new SQLiteImplementedBatchSource(context, inventorySource);
            operatorSource = new SQLiteBatchOperatorSource(context);
            viewModel = new ReceivingHistoryViewModel(
                receivedBatchSource,
                inventorySource,
                operatorSource,
                implementedBatchSource,
                GetReceivingManagementViewModel()
            );
            command = new ChangeSearchCriteriaPanelVisibilityCommand(viewModel);
        }

        ReceivingManagementViewModel GetReceivingManagementViewModel()
        {
            return new ReceivingManagementViewModel(
                new DuracolorIntermixBatchNumberValidator(),
                colorList: new DuracolorIntermixColorList(),
                receivingSource: receivedBatchSource,
                operatorSource: operatorSource,
                inventorySource: inventorySource
            );
        }

        [TearDown]
        public void TearDown()
        {
            sqliteHelper.DestroyTestDatabase();
        }
    }
}
