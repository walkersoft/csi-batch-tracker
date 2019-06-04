namespace CSI.BatchTracker.ViewModels.Commands
{
    public sealed class ChangeSearchCriteriaPanelVisibilityCommand : CommandBase
    {
        ReceivingHistoryViewModel viewModel;

        public ChangeSearchCriteriaPanelVisibilityCommand(ReceivingHistoryViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.SearchCriteriaVisibilityManagerIsSet();
        }

        public override void Execute(object parameter)
        {
            viewModel.SetActiveSearchCritera();
        }
    }
}
