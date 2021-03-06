﻿namespace CSI.BatchTracker.ViewModels.Commands
{
    public sealed class DeletePurchaseOrderReceivingRecordCommand : CommandBase
    {
        ReceivedPurchaseOrderEditorViewModel viewModel;

        public DeletePurchaseOrderReceivingRecordCommand(ReceivedPurchaseOrderEditorViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.SelectedReceivedBatchCanBeDeleted();
        }

        public override void Execute(object parameter)
        {
            viewModel.DeleteSelectedReceivingRecordFromLedger();
        }
    }
}
