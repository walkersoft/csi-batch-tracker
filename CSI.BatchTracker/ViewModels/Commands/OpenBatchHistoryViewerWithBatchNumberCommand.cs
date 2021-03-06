﻿namespace CSI.BatchTracker.ViewModels.Commands
{
    public sealed class OpenBatchHistoryViewerWithBatchNumberCommand : CommandBase
    {
        MainWindowViewModel viewModel;

        public OpenBatchHistoryViewerWithBatchNumberCommand(MainWindowViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.BatchHistoryViewerIsSetAndImplementedBatchIsSelected();
        }

        public override void Execute(object parameter)
        {
            viewModel.ShowBatchHistoryViewerWithBatchNumber();
        }
    }
}
