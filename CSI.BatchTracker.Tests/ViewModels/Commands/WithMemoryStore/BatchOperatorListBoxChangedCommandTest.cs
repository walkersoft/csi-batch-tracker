using CSI.BatchTracker.Domain.DataSource.MemorySource;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.WithMemoryStore
{
    [TestFixture]
    class BatchOperatorListBoxChangedCommandTest : BatchOperatorListBoxChangedCommandBehaviorTest
    {
        [SetUp]
        public override void SetUp()
        {
            MemoryStoreContext store = new MemoryStoreContext();
            operatorSource = new MemoryBatchOperatorSource(store);
            base.SetUp();
        }
    }
}
