namespace CSI.BatchTracker.ViewModels.Commands
{
    public sealed class ListReceivingRecordsByDateRangeCommand : CommandBase
    {
        ReceivingHistoryViewModel viewModel;

        public ListReceivingRecordsByDateRangeCommand(ReceivingHistoryViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.DateRangeCriteriaIsMet();
        }

        public override void Execute(object parameter)
        {
            viewModel.FetchReceivingRecordsByDateRange();
        }
    }
}
