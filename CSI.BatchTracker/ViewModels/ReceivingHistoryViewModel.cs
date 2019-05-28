using CSI.BatchTracker.Domain.DataSource.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.ViewModels
{
    public sealed class ReceivingHistoryViewModel : ViewModelBase
    {
        IReceivedBatchSource receivedBatchSource;
        IActiveInventorySource inventorySource;

        public DateTime DateRangeStartingDate { get; set; }
        public DateTime DateRangeEndingDate { get; set; }

        public ReceivingHistoryViewModel(IReceivedBatchSource receivedBatchSource, IActiveInventorySource inventorySource)
        {
            this.receivedBatchSource = receivedBatchSource;
            this.inventorySource = inventorySource;
        }

        public bool DateRangeCriteriaIsMet()
        {
            return DateRangeStartingDate > DateTime.MinValue
                && DateRangeEndingDate > DateTime.MinValue
                && DateRangeIsTheSameDayOfTheSameYear();
        }

        bool DateRangeIsTheSameDayOfTheSameYear()
        {
            return DateRangeStartingDate.DayOfYear == DateRangeEndingDate.DayOfYear
                && DateRangeStartingDate.Year == DateRangeEndingDate.Year;
        }
    }
}
