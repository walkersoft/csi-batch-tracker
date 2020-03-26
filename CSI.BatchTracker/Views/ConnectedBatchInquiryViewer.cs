using CSI.BatchTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
