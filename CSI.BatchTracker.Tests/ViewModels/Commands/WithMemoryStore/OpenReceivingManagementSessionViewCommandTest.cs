using CSI.BatchTracker.Domain.DataSource.MemorySource;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors;
using CSI.BatchTracker.ViewModels;
using CSI.BatchTracker.ViewModels.Commands;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.WithMemoryStore
{
    [TestFixture]
    class OpenReceivingManagementSessionViewCommandTest : OpenReceivingManagementSessionViewCommandBehaviorTest
    {
        [SetUp]
        public override void SetUp()
        {
            MemoryStoreContext context = new MemoryStoreContext();
            inventorySource = new MemoryActiveInventorySource(context);
            receivedBatchSource = new MemoryReceivedBatchSource(context, inventorySource);
            implementedBatchSource = new MemoryImplementedBatchSource(context, inventorySource);
            operatorSource = new MemoryBatchOperatorSource(context);
            viewModel = new MainWindowViewModel(inventorySource, receivedBatchSource, implementedBatchSource, operatorSource);
            command = new OpenReceivingManagementSessionViewCommand(viewModel);
            base.SetUp();
        }
    }
}
