using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.ViewModels.Commands
{
    public class ListReceivingRecordsByDateRangeCommand : CommandBase
    {
        ReceivingHistoryViewModel viewModel;

        public ListReceivingRecordsByDateRangeCommand(ReceivingHistoryViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.DateRangeCriteriaIsMet();
        }

        public override void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
