namespace CSI.BatchTracker.ViewModels.Commands
{
    public sealed class DeleteSelectedBatchOperatorCommand : CommandBase
    {
        BatchOperatorViewModel viewModel;

        public DeleteSelectedBatchOperatorCommand(BatchOperatorViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.BatchOperatorIsRemoveable();
        }

        public override void Execute(object parameter)
        {
            viewModel.RemoveSelectedBatchOperator();
        }
    }
}
