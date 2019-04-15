namespace CSI.BatchTracker.ViewModels.Commands
{
    public sealed class OpenBatchHistoryViewerCommand : CommandBase
    {
        MainWindowViewModel viewModel;

        public OpenBatchHistoryViewerCommand(MainWindowViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.BatchHistoryViewerIsSet();
        }

        public override void Execute(object parameter)
        {
            viewModel.ShowBatchHistoryViewer();
        }
    }
}
