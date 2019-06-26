using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.ViewModels.Commands
{
    public sealed class OpenReceivingRecordForViewingCommand : CommandBase
    {
        ReceivingHistoryViewModel viewModel;

        public OpenReceivingRecordForViewingCommand(ReceivingHistoryViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.ReceivedPurchaseOrderIsSelectedAndReceivingSessionIsReady();
        }

        public override void Execute(object parameter)
        {
            viewModel.OpenReceivingSessionToViewSelectedPurchaseOrder();
        }
    }
}
