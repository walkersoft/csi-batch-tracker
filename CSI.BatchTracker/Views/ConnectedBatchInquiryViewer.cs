using CSI.BatchTracker.ViewModels;

namespace CSI.BatchTracker.Views
{
    public class ConnectedBatchInquiryViewer : ViewBase
    {
        ImplementationInquiryViewModel viewModel;

        public ConnectedBatchInquiryViewer(ImplementationInquiryViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void ResetWindow()
        {
            window = new ConnectedBatchInquiryWindow(viewModel);
        }
    }
}
