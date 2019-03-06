using CSI.BatchTracker.Domain.DataSource.MemorySource;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Tests.Domain.DataSource.Behaviors;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.Storage.MemoryStore
{
    [TestFixture]
    class MemoryBatchOperatorStoreTest : BatchOperatorSourceBehaviorTest
    {
        [SetUp]
        public override void SetUp()
        {
            operatorSource = new MemoryBatchOperatorSource(new MemoryStoreContext());
            base.SetUp();
        }
    }
}
