using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.ViewModels.Commands
{
    public sealed class CommitBatchToInventoryLedgerCommand : CommandBase
    {
        MainWindowViewModel viewModel;

        public CommitBatchToInventoryLedgerCommand(MainWindowViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.BatchIsSetupForImplementation();
        }

        public override void Execute(object parameter)
        {
            viewModel.ImplementBatchFromInventoryToImplementationLedger();
        }
    }
}
