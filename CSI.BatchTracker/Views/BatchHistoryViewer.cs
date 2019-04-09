using CSI.BatchTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Views
{
    public sealed class BatchHistoryViewer : ViewBase
    {
        BatchHistoryViewModel viewModel;
        public string IncomingBatchNumber { get; set; }

        public BatchHistoryViewer(BatchHistoryViewModel viewModel)
        {
            this.viewModel = viewModel;
            PopulateViewModelBatchFromIncomingBatchNumberIfPresent();
        }

        public override void ResetWindow()
        {
            window = new BatchHistoryWindow(viewModel);
        }

        void PopulateViewModelBatchFromIncomingBatchNumberIfPresent()
        {
            if (string.IsNullOrEmpty(IncomingBatchNumber) == false)
            {
                viewModel.BatchNumber = IncomingBatchNumber;
                IncomingBatchNumber = string.Empty;
            }
        }
    }
}
