using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Tests.Views;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors
{
    [TestFixture]
    abstract class OpenPurchaseOrderEditorCommandBehaviorTest : ReceivingHistoryViewModelCommandBehaviorTestingBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [Test]
        public void CommandWillNotExecuteIfViewIsNotSet()
        {
            viewModel.PurchaseOrderEditorViewer = null;
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillNotExecuteIfViewCannotShowItself()
        {
            viewModel.PurchaseOrderEditorViewer = new IViewTestStub();
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillExecuteIfViewIsSetAndCanShowItselfAndARetrievedRecordIsSet()
        {
            int targetPO = 11111;
            ReceivedBatch receivedBatch = helper.GetBatchWithSpecificPO(targetPO);
            BatchOperator batchOperator = receivedBatch.ReceivingOperator;

            operatorSource.SaveOperator(batchOperator);
            receivedBatchSource.SaveReceivedBatch(receivedBatch);
            viewModel.PurchaseOrderEditorViewer = new PassableIViewTestStub();
            viewModel.SpecificPONumber = targetPO.ToString();
            viewModel.FetchReceivingRecordsByPONumber();

            Assert.True(command.CanExecute(null));
        }

        [Test]
        public void ExecutedCommandWillCallIViewShowViewMethod()
        {
            int targetPO = 11111;
            ReceivedBatch receivedBatch = helper.GetBatchWithSpecificPO(targetPO);
            BatchOperator batchOperator = receivedBatch.ReceivingOperator;

            operatorSource.SaveOperator(batchOperator);
            receivedBatchSource.SaveReceivedBatch(receivedBatch);
            viewModel.PurchaseOrderEditorViewer = new IViewTestStub();
            viewModel.SpecificPONumber = targetPO.ToString();
            viewModel.FetchReceivingRecordsByPONumber();

            Assert.DoesNotThrow(() => command.Execute(null));
        }
    }
}
