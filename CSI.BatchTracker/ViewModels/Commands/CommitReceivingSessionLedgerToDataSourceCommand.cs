namespace CSI.BatchTracker.ViewModels.Commands
{
    public sealed class CommitReceivingSessionLedgerToDataSourceCommand : CommandBase
    {
        ReceivingManagementViewModel viewModel;

        public CommitReceivingSessionLedgerToDataSourceCommand(ReceivingManagementViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.SessionLedger.Count > 0;
        }

        public override void Execute(object parameter)
        {
            viewModel.CommitSessionLedgerToDataSource();
        }
    }
}
