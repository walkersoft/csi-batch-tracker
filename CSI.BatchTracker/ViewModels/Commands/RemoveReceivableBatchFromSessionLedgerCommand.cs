namespace CSI.BatchTracker.ViewModels.Commands
{
    public class RemoveReceivableBatchFromSessionLedgerCommand : CommandBase
    {
        ReceivingManagementViewModel viewModel;

        public RemoveReceivableBatchFromSessionLedgerCommand(ReceivingManagementViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.SessionLedgerSelectedItemCanBeRemoved();
        }

        public override void Execute(object parameter)
        {
            viewModel.RemoveSelectedEntryFromSessionLedger();
        }
    }
}
