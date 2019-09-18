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
    class DeletePurchaseOrderReceivingRecordCommandTest : DeletePurchaseOrderReceivingRecordCommandBehaviorTest
    {
        [SetUp]
        public override void SetUp()
        {
            MemoryStoreContext context = new MemoryStoreContext();
            operatorSource = new MemoryBatchOperatorSource(context);
            inventorySource = new MemoryActiveInventorySource(context);
            receivedBatchSource = new MemoryReceivedBatchSource(context, inventorySource);
            implementedBatchSource = new MemoryImplementedBatchSource(context, inventorySource);
            base.SetUp();

            viewModel = new ReceivedPurchaseOrderEditorViewModel(
                receivedBatchSource.GetPurchaseOrderForEditing(originalPONumber),
                new DuracolorIntermixColorList(),
                new DuracolorIntermixBatchNumberValidator(),
                operatorSource,
                inventorySource,
                receivedBatchSource,
                implementedBatchSource
            );

            command = new DeletePurchaseOrderReceivingRecordCommand(viewModel);
        }
    }
}
