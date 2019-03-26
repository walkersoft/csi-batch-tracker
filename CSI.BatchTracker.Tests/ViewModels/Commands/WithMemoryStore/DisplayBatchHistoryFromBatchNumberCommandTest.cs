using CSI.BatchTracker.Domain;
using CSI.BatchTracker.Domain.DataSource.MemorySource;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors;
using CSI.BatchTracker.ViewModels;
using CSI.BatchTracker.ViewModels.Commands;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.WithMemoryStore
{
    [TestFixture]
    class DisplayBatchHistoryFromBatchNumberCommandTest : DisplayBatchHistoryFromBatchNumberCommandBehaviorTest
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            validator = new DuracolorIntermixBatchNumberValidator();
            MemoryStoreContext context = new MemoryStoreContext();
            inventorySource = new MemoryActiveInventorySource(context);
            receivedBatchSource = new MemoryReceivedBatchSource(context, inventorySource);
            implementedBatchSource = new MemoryImplementedBatchSource(context, inventorySource);
            viewModel = new BatchHistoryViewModel(validator, inventorySource, receivedBatchSource, implementedBatchSource);
            command = new DisplayBatchHistoryFromBatchNumberCommand(viewModel);
        }
    }
}
