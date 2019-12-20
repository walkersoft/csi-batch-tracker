using CSI.BatchTracker.Domain.DataSource.MemorySource;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Tests.Domain.DataSource.Behaviors;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.Storage.MemoryStore
{
    [TestFixture]
    class MemoryActiveInventorySourceTest : ActiveInventorySourceBehaviorTest
    {
        [SetUp]
        public override void SetUp()
        {
            MemoryStoreContext context = new MemoryStoreContext();
            inventorySource = new MemoryActiveInventorySource(context);
            operatorSource = new MemoryBatchOperatorSource(context);
            base.SetUp();
        }
    }
}
