namespace CSI.BatchTracker.ViewModels.Commands
{
    public sealed class SaveBatchOperatorCommand : CommandBase
    {
        BatchOperatorViewModel viewModel;

        public SaveBatchOperatorCommand(BatchOperatorViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.BatchOperatorIsValid();
        }

        public override void Execute(object parameter)
        {
            viewModel.PersistBatchOperator();
        }
    }
}
