namespace CSI.BatchTracker.ViewModels.Commands
{
    public sealed class UndoImplementedBatchCommand : CommandBase
    {
        MainWindowViewModel viewModel;

        public UndoImplementedBatchCommand(MainWindowViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.SelectedBatchCanBeUndone();
        }

        public override void Execute(object parameter)
        {
            viewModel.UndoSelectedBatchFromImplementationLedger();
        }
    }
}
