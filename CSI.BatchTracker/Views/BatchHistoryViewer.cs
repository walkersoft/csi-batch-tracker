using CSI.BatchTracker.ViewModels;
using CSI.BatchTracker.Views.Contracts;

namespace CSI.BatchTracker.Views
{
    public sealed class BatchHistoryViewer : ViewBase, IBatchHistoryView
    {
        BatchHistoryViewModel viewModel;
        public string IncomingBatchNumber { get; set; }

        public BatchHistoryViewer(BatchHistoryViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void ResetWindow()
        {
            window = new BatchHistoryWindow(viewModel);
            PopulateViewModelBatchFromIncomingBatchNumberIfPresent();
        }

        void PopulateViewModelBatchFromIncomingBatchNumberIfPresent()
        {
            viewModel.BatchNumber = IncomingBatchNumber;

            if (viewModel.BatchNumberIsValid())
            {
                viewModel.RetrieveBatchHistoryData();
                IncomingBatchNumber = string.Empty;
            }
        }
    }
}
