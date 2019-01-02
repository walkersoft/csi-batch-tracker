namespace CSI.BatchTracker.ViewModels.Commands
{
    public sealed class BatchOperatorComboBoxChangedCommand : CommandBase
    {
        BatchOperatorViewModel viewModel;

        public BatchOperatorComboBoxChangedCommand(BatchOperatorViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.SelectedBatchOperatorFromComboBoxIndex > -1;
        }

        public override void Execute(object parameter)
        {
            viewModel.PopulateBatchOperatorOrReset();
        }
    }
}
