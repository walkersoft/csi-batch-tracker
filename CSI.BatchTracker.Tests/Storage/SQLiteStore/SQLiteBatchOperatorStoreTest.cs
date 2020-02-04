using CSI.BatchTracker.Domain.DataSource.SQLiteStore;
using CSI.BatchTracker.Storage.SQLiteStore;
using CSI.BatchTracker.Tests.Domain.DataSource.Behaviors;
using CSI.BatchTracker.Tests.TestHelpers.Storage.SQLiteStore;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.Storage.SQLiteStore
{
    [TestFixture]
    class SQLiteBatchOperatorStoreTest : BatchOperatorSourceBehaviorTest
    {
        SQLiteDatabaseHelper databaseHelper;

        [SetUp]
        public override void SetUp()
        {
            databaseHelper = new SQLiteDatabaseHelper();
            databaseHelper.CreateTestDatabase();
            operatorSource = new SQLiteBatchOperatorSource(new SQLiteStoreContext(databaseHelper.DatabaseFile));
            base.SetUp();
        }

        [TearDown]
        public void TearDown()
        {
            databaseHelper.DestroyTestDatabase();
        }
    }
}
