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
            dataSource = new MemoryReceivedBatchSource(new MemoryStoreContext());
            base.SetUp();
        }
    }
}
