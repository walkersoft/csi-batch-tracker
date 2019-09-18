using CSI.BatchTracker.ViewModels;

namespace CSI.BatchTracker.Views
{
    public sealed class ReceivedPurchaseOrderEditorViewer : ViewBase
    {
        ReceivingHistoryViewModel viewModel;

        public ReceivedPurchaseOrderEditorViewer(ReceivingHistoryViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void ResetWindow()
        {
            window = new ReceivedPurchaseOrderEditorWindow(viewModel);
        }
    }
}
