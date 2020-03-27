namespace CSI.BatchTracker.ViewModels.Commands
{
    public class OpenConnectedBatchInquiryViewCommand : CommandBase
    {
        MainWindowViewModel viewModel;

        public OpenConnectedBatchInquiryViewCommand(MainWindowViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.ConnectedBatchViewerIsSet();
        }

        public override void Execute(object parameter)
        {
            viewModel.ConnectedBatchInquiryViewer.ShowView();
        }
    }
}
