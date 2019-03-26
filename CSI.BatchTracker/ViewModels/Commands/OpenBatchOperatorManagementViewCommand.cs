namespace CSI.BatchTracker.ViewModels.Commands
{
    public sealed class OpenBatchOperatorManagementViewCommand : CommandBase
    {
        MainWindowViewModel viewModel;

        public OpenBatchOperatorManagementViewCommand(MainWindowViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.BatchOperatorManagementSessionViewIsSet();
        }

        public override void Execute(object parameter)
        {
            viewModel.ShowBatchOperatorManagementSessionView();
        }
    }
}
