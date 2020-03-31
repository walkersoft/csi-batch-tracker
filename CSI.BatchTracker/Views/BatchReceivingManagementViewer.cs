using CSI.BatchTracker.ViewModels;

namespace CSI.BatchTracker.Views
{
    public sealed class BatchReceivingManagementViewer : ViewBase
    {
        ReceivingManagementViewModel viewModel;

        public BatchReceivingManagementViewer(ReceivingManagementViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void ResetWindow()
        {
            window = new BatchReceivingManagementWindow(viewModel);
        }        
    }
}
