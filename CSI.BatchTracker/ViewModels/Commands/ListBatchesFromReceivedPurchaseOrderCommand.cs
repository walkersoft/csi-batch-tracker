namespace CSI.BatchTracker.ViewModels.Commands
{
    public class ListBatchesFromReceivedPurchaseOrderCommand : CommandBase
    {
        ReceivingHistoryViewModel viewModel;

        public ListBatchesFromReceivedPurchaseOrderCommand(ReceivingHistoryViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.ReceivedPurchaseOrderIsSelected();
        }

        public override void Execute(object parameter)
        {
            viewModel.PopulateSelectedPurchaseOrderBatchCollection();
        }
    }
}
