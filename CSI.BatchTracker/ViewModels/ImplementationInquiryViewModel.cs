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
    public class ImplementationInquiryViewModel
    {
        public DateTime SelectedDate { get; set; }
        public ObservableCollection<LoggedBatch> ImplementedBatches { get; private set; }

        IImplementedBatchSource implementedBatchSource;

        public ImplementationInquiryViewModel(IImplementedBatchSource implementedBatchSource)
        {
            this.implementedBatchSource = implementedBatchSource;
            ImplementedBatches = new ObservableCollection<LoggedBatch>();
        }

        public bool InquiryReadyForSelectionByDate()
        {
            return SelectedDate > DateTime.MinValue;
        }

        public void GetConnectedBatchesAtSpecifiedDate()
        {
            ImplementedBatches = implementedBatchSource.GetConnectedBatchesAtDate(SelectedDate);
        }
    }
}
