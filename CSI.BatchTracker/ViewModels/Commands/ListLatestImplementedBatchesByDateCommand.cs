﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.ViewModels.Commands
{
    public class ListLatestImplementedBatchesByDateCommand : CommandBase
    {
        ImplementationInquiryViewModel viewModel;

        public ListLatestImplementedBatchesByDateCommand(ImplementationInquiryViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.InquiryReadyForSelectionByDate();
        }

        public override void Execute(object parameter)
        {
            viewModel.GetConnectedBatchesAtSpecifiedDate();
        }
    }
}
