using CSI.BatchTracker.Domain.DataSource.MemorySource;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Tests.Domain.DataSource;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.Storage.MemoryStore
{
    [TestFixture]
    class MemoryReceivedBatchStoreTest : ReceivedBatchSourceBehaviorTest
    {
        [SetUp]
        public override void SetUp()
        {
            MemoryStoreContext context = new MemoryStoreContext();
            inventorySource = new MemoryActiveInventorySource(context);
            dataSource = new MemoryReceivedBatchSource(context, inventorySource);
            base.SetUp();
        }
    }
}
