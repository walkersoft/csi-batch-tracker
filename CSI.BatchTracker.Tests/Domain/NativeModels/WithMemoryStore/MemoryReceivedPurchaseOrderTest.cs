using CSI.BatchTracker.Domain.DataSource.MemorySource;
using CSI.BatchTracker.Storage.MemoryStore;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.Domain.NativeModels.WithMemoryStore
{
    [TestFixture]
    class MemoryReceivedPurchaseOrderTest : ReceivedPurchaseOrderTest
    {
        [SetUp]
        public override void SetUp()
        {
            operatorSource = new MemoryBatchOperatorSource(new MemoryStoreContext());
            base.SetUp();
        }
    }
}
