using CSI.BatchTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Views
{
    public class BatchReceivingManagementViewer : ViewBase
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
