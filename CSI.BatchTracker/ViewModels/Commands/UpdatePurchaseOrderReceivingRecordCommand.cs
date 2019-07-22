﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.ViewModels.Commands
{
    public sealed class UpdatePurchaseOrderReceivingRecordCommand : CommandBase
    {
        ReceivedPurchaseOrderEditorViewModel viewModel;

        public UpdatePurchaseOrderReceivingRecordCommand(ReceivedPurchaseOrderEditorViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.SelectedReceivingRecordIsReadyForUpdate();
        }

        public override void Execute(object parameter)
        {
            viewModel.UpdateSelectedReceivingRecord();
        }
    }
}