using CSI.BatchTracker.Domain.DataSource.SQLiteStore;
using CSI.BatchTracker.Storage.SQLiteStore;
using CSI.BatchTracker.Tests.Domain.DataSource.Behaviors;
using CSI.BatchTracker.Tests.TestHelpers.Storage.SQLiteStore;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.Storage.SQLiteStore
{
    [TestFixture]
    class SQLiteImplementedBatchSourceTest : ImplementedBatchSourceBehaviorTest
    {
        SQLiteDatabaseHelper databaseHelper;
        SQLiteStoreContext context;

        [SetUp]
        public override void SetUp()
        {
            databaseHelper = new SQLiteDatabaseHelper();
            databaseHelper.CreateTestDatabase();
            context = new SQLiteStoreContext(databaseHelper.DatabaseFile);
            inventorySource = new SQLiteActiveInventorySource(context);
            operatorSource = new SQLiteBatchOperatorSource(context);
            implementedBatchSource = new SQLiteImplementedBatchSource(context, inventorySource);
            base.SetUp();
        }
    }
}
