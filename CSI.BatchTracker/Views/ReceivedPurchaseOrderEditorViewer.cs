using CSI.BatchTracker.ViewModels;
using CSI.BatchTracker.Views.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Views
{
    public sealed class ReceivedPurchaseOrderEditorViewer : ViewBase
    {
        ReceivedPurchaseOrderEditorViewModel viewModel;

        public ReceivedPurchaseOrderEditorViewer(ReceivedPurchaseOrderEditorViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void ResetWindow()
        {
            window = new ReceivedPurchaseOrderEditorWindow(viewModel);
        }
    }
}
