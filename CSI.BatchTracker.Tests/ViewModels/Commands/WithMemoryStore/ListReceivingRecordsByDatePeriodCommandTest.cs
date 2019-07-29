using CSI.BatchTracker.Domain.DataSource.MemorySource;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors;
using CSI.BatchTracker.ViewModels;
using CSI.BatchTracker.ViewModels.Commands;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.WithMemoryStore
{
    [TestFixture]
    class ListReceivingRecordsByDatePeriodCommandTest : ListReceivingRecordsByDatePeriodCommandBehaviorTest
    {
        [SetUp]
        public override void SetUp()
        {
            MemoryStoreContext context = new MemoryStoreContext();
            operatorSource = new MemoryBatchOperatorSource(context);
            inventorySource = new MemoryActiveInventorySource(context);
            receivedBatchSource = new MemoryReceivedBatchSource(context, inventorySource);
            viewModel = new ReceivingHistoryViewModel(
                receivedBatchSource,
                inventorySource,
                operatorSource,
                implementedBatchSource,
                GetReceivingManagementViewModel()
            );
            command = new ListReceivingRecordsByDatePeriodCommand(viewModel);
            base.SetUp();
        }
    }
}
