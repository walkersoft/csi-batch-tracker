using CSI.BatchTracker.Domain;
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
    class DeletePurchaseOrderReceivingRecordCommandTest : DeletePurchaseOrderReceivingRecordCommandBehaviorTest
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
            implementedBatchSource = new SQLiteImplementedBatchSource(context, inventorySource);
            operatorSource = new SQLiteBatchOperatorSource(context);
            base.SetUp();

            viewModel = new ReceivedPurchaseOrderEditorViewModel(
                receivedBatchSource.GetPurchaseOrderForEditing(originalPONumber),
                new DuracolorIntermixColorList(),
                new DuracolorIntermixBatchNumberValidator(),
                operatorSource,
                inventorySource,
                receivedBatchSource,
                implementedBatchSource
            );

            command = new DeletePurchaseOrderReceivingRecordCommand(viewModel);
        }

        [TearDown]
        public void TearDown()
        {
            sqliteHelper.DestroyTestDatabase();
        }
    }
}
