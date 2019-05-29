using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<ReceivedBatch> RetreivedRecordsLedger { get; set; }
        public int SearchCriteriaSelectedIndex { get; private set; }

        public ReceivingHistoryViewModel(IReceivedBatchSource receivedBatchSource, IActiveInventorySource inventorySource)
        {
            this.receivedBatchSource = receivedBatchSource;
            this.inventorySource = inventorySource;
            SearchCriteriaSelectedIndex = 0;
            NotifyPropertyChanged("SearchCriteriaSelectedIndex");
        }

        public bool DateRangeCriteriaIsMet()
        {
            return DateRangeStartingDate > DateTime.MinValue
                && DateRangeEndingDate > DateTime.MinValue
                && DateRangeStartingDateIsOnOrBeforeEndingDate();
        }

        bool DateRangeStartingDateIsOnOrBeforeEndingDate()
        {
            return DateRangeStartingDate.Date <= DateRangeEndingDate.Date;
        }

        public void FetchReceivingRecordsBasedOnSearchCriteria()
        {
            if (SearchCriteriaSelectedIndex == 0)
            {
                RetreivedRecordsLedger = receivedBatchSource.GetReceivedBatchesWithinDateRange(DateRangeStartingDate, DateRangeEndingDate);
                NotifyPropertyChanged("RetreivedRecordsLedger");
            }
        }
    }
}
