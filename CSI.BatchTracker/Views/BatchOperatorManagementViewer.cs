using CSI.BatchTracker.ViewModels;

namespace CSI.BatchTracker.Views
{
    public sealed class BatchOperatorManagementViewer : ViewBase
    {
        BatchOperatorViewModel viewModel;

        public BatchOperatorManagementViewer(BatchOperatorViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void ResetWindow()
        {
            window = new BatchOperatorManagementWindow(viewModel);
        }
    }
}
