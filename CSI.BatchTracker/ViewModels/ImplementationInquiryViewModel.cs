using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.ViewModels.Commands;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CSI.BatchTracker.ViewModels
{
    public sealed class ImplementationInquiryViewModel : ViewModelBase
    {
        IImplementedBatchSource implementedBatchSource;
        public DateTime SelectedDate { get; set; }
        public ObservableCollection<LoggedBatch> ImplementedBatches { get; private set; }
        public ICommand ShowConnectedBatchesCommand { get; private set; }

        public ImplementationInquiryViewModel(IImplementedBatchSource implementedBatchSource)
        {
            this.implementedBatchSource = implementedBatchSource;
            ImplementedBatches = new ObservableCollection<LoggedBatch>();
            ShowConnectedBatchesCommand = new ListLatestImplementedBatchesByDateCommand(this);
            SelectedDate = DateTime.Today;
        }

        public bool InquiryReadyForSelectionByDate()
        {
            return SelectedDate > DateTime.MinValue;
        }

        public void GetConnectedBatchesAtSpecifiedDate()
        {
            ImplementedBatches = implementedBatchSource.GetConnectedBatchesAtDate(SelectedDate);
            NotifyPropertyChanged("ImplementedBatches");
        }
    }
}
