using CSI.BatchTracker.Domain.DataSource.SQLiteStore;
using CSI.BatchTracker.Storage.SQLiteStore;
using CSI.BatchTracker.Tests.TestHelpers.Storage.SQLiteStore;
using CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors;
using CSI.BatchTracker.ViewModels;
using CSI.BatchTracker.ViewModels.Commands;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.WithSQLiteStore
{
    [TestFixture]
    class ListReceivingRecordsByDateRangeCommandTest : ListReceivingRecordsByDateRangeCommandBehaviorTest
    {
        SQLiteDatabaseHelper sqliteHelper;

        [SetUp]
        public override void SetUp()
        {
            sqliteHelper = new SQLiteDatabaseHelper();
            sqliteHelper.CreateTestDatabase();
            SQLiteStoreContext context = new SQLiteStoreContext(sqliteHelper.DatabaseFile);
            inventorySource = new SQLiteActiveInventorySource(context);
            receivedBatchSource = new SQLiteReceivedBatchSource(context, inventorySource);
            operatorSource = new SQLiteBatchOperatorSource(context);
            viewModel = new ReceivingHistoryViewModel(
                receivedBatchSource,
                inventorySource,
                operatorSource,
                implementedBatchSource,
                GetReceivingManagementViewModel()
            );
            command = new ListReceivingRecordsByDateRangeCommand(viewModel);
            base.SetUp();
        }

        [TearDown]
        public void TearDown()
        {
            sqliteHelper.DestroyTestDatabase();
        }
    }
}
