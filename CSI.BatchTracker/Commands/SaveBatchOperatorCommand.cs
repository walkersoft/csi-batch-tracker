using CSI.BatchTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CSI.BatchTracker.Commands
{
    public sealed class SaveBatchOperatorCommand : ICommand
    {
        BatchOperatorViewModel viewModel;

        public SaveBatchOperatorCommand(BatchOperatorViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return viewModel.BatchOperatorIsValid();
        }

        public void Execute(object parameter)
        {
            viewModel.PersistBatchOperator();
        }
    }
}
