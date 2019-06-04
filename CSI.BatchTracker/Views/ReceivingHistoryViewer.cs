using CSI.BatchTracker.ViewModels;

namespace CSI.BatchTracker.Views
{
    public sealed class ReceivingHistoryViewer : ViewBase
    {
        ReceivingHistoryViewModel viewModel;

        public ReceivingHistoryViewer(ReceivingHistoryViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void ResetWindow()
        {
            window = new ReceivingHistoryWindow(viewModel);
        }
    }
}
