namespace CSI.BatchTracker.ViewModels.Commands
{
    public sealed class DisplayBatchHistoryFromBatchNumberCommand : CommandBase
    {
        BatchHistoryViewModel viewModel;

        public DisplayBatchHistoryFromBatchNumberCommand(BatchHistoryViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.BatchNumberIsValid();
        }

        public override void Execute(object parameter)
        {
            viewModel.RetrieveBatchHistoryData();
        }
    }
}
