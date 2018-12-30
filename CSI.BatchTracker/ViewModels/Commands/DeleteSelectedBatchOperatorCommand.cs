using CSI.BatchTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.ViewModels.Commands
{
    public sealed class DeleteSelectedBatchOperatorCommand : CommandBase
    {
        BatchOperatorViewModel viewModel;

        public DeleteSelectedBatchOperatorCommand(BatchOperatorViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.BatchOperatorIsRemoveable();
        }

        public override void Execute(object parameter)
        {
            viewModel.RemoveSelectedBatchOperator();
        }
    }
}
