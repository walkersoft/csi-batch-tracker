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
            inventorySource = new MemoryActiveInventorySource(new MemoryStoreContext());
            base.SetUp();
        }
    }
}
