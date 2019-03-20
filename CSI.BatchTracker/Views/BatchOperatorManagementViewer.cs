using CSI.BatchTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
