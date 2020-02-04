using CSI.BatchTracker.Domain.DataSource.SQLiteStore;
using CSI.BatchTracker.Storage.SQLiteStore;
using CSI.BatchTracker.Tests.TestHelpers.Storage.SQLiteStore;
using CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.WithSQLiteStore
{
    [TestFixture]
    class BatchOperatorComboBoxChangedCommandTest : BatchOperatorComboBoxChangedCommandBehaviorTest
    {
        SQLiteDatabaseHelper sqliteHelper;

        [SetUp]
        public override void SetUp()
        {
            sqliteHelper = new SQLiteDatabaseHelper();
            sqliteHelper.CreateTestDatabase();
            SQLiteStoreContext context = new SQLiteStoreContext(sqliteHelper.DatabaseFile);
            operatorSource = new SQLiteBatchOperatorSource(context);
            base.SetUp();
        }

        [TearDown]
        public void TearDown()
        {
            sqliteHelper.DestroyTestDatabase();
        }
    }
}
