﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.ViewModels.Commands
{
    public sealed class OpenPurchaseOrderEditorCommand : CommandBase
    {
        ReceivingHistoryViewModel viewModel;

        public OpenPurchaseOrderEditorCommand(ReceivingHistoryViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.PurchaseOrderEditorViewIsSet();
        }

        public override void Execute(object parameter)
        {
            viewModel.ShowPurchaseOrderEditorView();
        }
    }
}
