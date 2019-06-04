namespace CSI.BatchTracker.ViewModels.Commands
{
    public sealed class OpenReceivingHistorySessionViewCommand : CommandBase
    {
        MainWindowViewModel viewModel;

        public OpenReceivingHistorySessionViewCommand(MainWindowViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.ReceivingHistoryViewerIsSet();
        }

        public override void Execute(object parameter)
        {
            viewModel.ShowReceivingHistoryViewer();
        }
    }
}
