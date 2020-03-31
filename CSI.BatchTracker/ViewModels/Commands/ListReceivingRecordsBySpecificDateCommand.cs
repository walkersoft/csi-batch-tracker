namespace CSI.BatchTracker.ViewModels.Commands
{
    public sealed class ListReceivingRecordsBySpecificDateCommand : CommandBase
    {
        ReceivingHistoryViewModel viewModel;

        public ListReceivingRecordsBySpecificDateCommand(ReceivingHistoryViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.SpecifcDateCriteriaIsSet();
        }

        public override void Execute(object parameter)
        {
            viewModel.FetchReceivingRecordsBySpecificDate();
        }
    }
}
