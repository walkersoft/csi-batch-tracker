using CSI.BatchTracker.Domain.DataSource.MemorySource;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.WithMemoryStore
{
    [TestFixture]
    class CommitReceivingSessionLedgerToDataSourceCommandTest : CommitReceivingSessionLedgerToDataSourceCommandBehaviorTest
    {
        [SetUp]
        public override void SetUp()
        {
            MemoryStoreContext context = new MemoryStoreContext();
            operatorSource = new MemoryBatchOperatorSource(context);
            inventorySource = new MemoryActiveInventorySource(context);
            receivingSource = new MemoryReceivedBatchSource(context, inventorySource);
            base.SetUp();
        }
    }
}
