using CSI.BatchTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CSI.BatchTracker.Commands
{
    public sealed class BatchOperatorComboBoxChangedCommand : CommandBase
    {
        BatchOperatorViewModel viewModel;

        public BatchOperatorComboBoxChangedCommand(BatchOperatorViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.UserSelectedComboBoxIndex > -1;
        }

        public override void Execute(object parameter)
        {
            viewModel.PopulateBatchOperatorOrCreateNew();
        }
    }
}
