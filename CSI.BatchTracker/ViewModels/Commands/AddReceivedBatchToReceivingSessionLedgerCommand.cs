using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.ViewModels.Commands
{
    public sealed class AddReceivedBatchToReceivingSessionLedgerCommand : CommandBase
    {
        ReceivingManagementViewModel viewModel;

        public AddReceivedBatchToReceivingSessionLedgerCommand(ReceivingManagementViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.ReceivedBatchIsValidForSessionLedger();
        }

        public override void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
