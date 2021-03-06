﻿namespace CSI.BatchTracker.ViewModels.Commands
{
    public sealed class CommitBatchToImplementationLedgerCommand : CommandBase
    {
        MainWindowViewModel viewModel;

        public CommitBatchToImplementationLedgerCommand(MainWindowViewModel viewModel)
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
