using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.ViewModels.Commands
{
    public sealed class UpdatePurchaseOrderHeaderCommand : CommandBase
    {
        ReceivedPurchaseOrderEditorViewModel viewModel;

        public UpdatePurchaseOrderHeaderCommand(ReceivedPurchaseOrderEditorViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.AllHeaderFieldsArePopulated();
        }

        public override void Execute(object parameter)
        {
            viewModel.UpdatePurchaseOrderHeaderInformation();
        }
    }
}
