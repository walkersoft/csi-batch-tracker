namespace CSI.BatchTracker.ViewModels.Commands
{
    public sealed class UpdatePurchaseOrderHeaderCommand : CommandBase
    {
        ReceivedPurchaseOrderEditorViewModel viewModel;

        public UpdatePurchaseOrderHeaderCommand(ReceivedPurchaseOrderEditorViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.AllHeaderFieldsArePopulated();
        }

        public override void Execute(object parameter)
        {
            viewModel.UpdatePurchaseOrderHeaderInformation();
        }
    }
}
