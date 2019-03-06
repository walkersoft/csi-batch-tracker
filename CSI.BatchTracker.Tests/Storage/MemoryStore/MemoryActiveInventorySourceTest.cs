using CSI.BatchTracker.Domain.DataSource.MemorySource;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Tests.Domain.DataSource;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.Storage.MemoryStore
{
    [TestFixture]
    class MemoryActiveInventorySourceTest : ActiveInventorySourceBehaviorTest
    {
        [SetUp]
        public override void SetUp()
        {
            dataSource = new MemoryActiveInventorySource(new MemoryStoreContext());
            base.SetUp();
        }
    }
}
