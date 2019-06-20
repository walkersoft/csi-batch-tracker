namespace CSI.BatchTracker.ViewModels.Commands
{
    public sealed class ListReceivingRecordsByDatePeriodCommand : CommandBase
    {
        ReceivingHistoryViewModel viewModel;

        public ListReceivingRecordsByDatePeriodCommand(ReceivingHistoryViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            viewModel.FetchReceivingRecordsBasedOnSearchCriteria();
        }
    }
}
