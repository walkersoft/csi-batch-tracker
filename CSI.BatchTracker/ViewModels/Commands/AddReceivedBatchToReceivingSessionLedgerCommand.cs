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
            viewModel.AddReceivedBatchToSessionLedger();
        }
    }
}
