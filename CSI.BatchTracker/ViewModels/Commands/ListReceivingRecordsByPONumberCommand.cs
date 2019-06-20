namespace CSI.BatchTracker.ViewModels.Commands
{
    public class ListReceivingRecordsByPONumberCommand : CommandBase
    {
        ReceivingHistoryViewModel viewModel;

        public ListReceivingRecordsByPONumberCommand(ReceivingHistoryViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.PONumberIsValid();
        }

        public override void Execute(object parameter)
        {
            viewModel.FetchReceivingRecordsByPONumber();
        }
    }
}
