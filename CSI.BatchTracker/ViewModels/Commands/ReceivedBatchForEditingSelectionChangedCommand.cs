namespace CSI.BatchTracker.ViewModels.Commands
{
    public sealed class ReceivedBatchForEditingSelectionChangedCommand : CommandBase
    {
        ReceivedPurchaseOrderEditorViewModel viewModel;

        public ReceivedBatchForEditingSelectionChangedCommand(ReceivedPurchaseOrderEditorViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.ReceivedRecordIsSelected();
        }

        public override void Execute(object parameter)
        {
            viewModel.PopulateSelectedReceivedRecord();
        }
    }
}
