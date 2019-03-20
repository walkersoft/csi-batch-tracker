using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.ViewModels.Commands
{
    public sealed class OpenReceivingManagementSessionViewCommand : CommandBase
    {
        MainWindowViewModel viewModel;

        public OpenReceivingManagementSessionViewCommand(MainWindowViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.ReceivingManagementSessionViewIsSet();
        }

        public override void Execute(object parameter)
        {
            viewModel.ShowReceivingManagementSessionView();
        }
    }
}
