namespace CSI.BatchTracker.ViewModels.Commands
{
    public sealed class BatchOperatorListBoxChangedCommand : CommandBase
    {
        BatchOperatorViewModel viewModel;

        public BatchOperatorListBoxChangedCommand(BatchOperatorViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.SelectedBatchOperatorFromListBoxIndex > -1;
        }

        public override void Execute(object parameter)
        {
            viewModel.MatchComboBoxOperatorWithListBoxOperator();
        }
    }
}
