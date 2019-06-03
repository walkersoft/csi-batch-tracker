using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CSI.BatchTracker.ViewModels
{
    public sealed class ReceivingHistoryViewModel : ViewModelBase
    {
        IReceivedBatchSource receivedBatchSource;
        IActiveInventorySource inventorySource;

        public DateTime DateRangeStartingDate { get; set; }
        public DateTime DateRangeEndingDate { get; set; }
        public ObservableCollection<ReceivedBatch> SelectedPurchaseOrderReceivedBatches { get; set; }
        public ObservableCollection<ReceivedPurchaseOrder> RetreivedRecordsLedger { get; set; }
        public int SearchCriteriaSelectedIndex { get; set; }
        public int RetreivedRecordsLedgerSelectedIndex { get; set; }

        public ICommand ListReceivingRecordsByDateRange { get; private set; }
        public ICommand ListBatchesFromReceivedPurchaseOrder { get; private set; }

        public ReceivingHistoryViewModel(IReceivedBatchSource receivedBatchSource, IActiveInventorySource inventorySource)
        {
            this.receivedBatchSource = receivedBatchSource;
            this.inventorySource = inventorySource;
            SearchCriteriaSelectedIndex = 0;
            DateRangeStartingDate = DateTime.Today;
            DateRangeEndingDate = DateTime.Today;
            ListReceivingRecordsByDateRange = new ListReceivingRecordsByDateRangeCommand(this);
            ListBatchesFromReceivedPurchaseOrder = new ListBatchesFromReceivedPurchaseOrderCommand(this);
            RetreivedRecordsLedger = new ObservableCollection<ReceivedPurchaseOrder>();
            SelectedPurchaseOrderReceivedBatches = new ObservableCollection<ReceivedBatch>();
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
                RetreivedRecordsLedger = AggregateRecordsByPONumber(
                    receivedBatchSource.GetReceivedBatchesWithinDateRange(DateRangeStartingDate, DateRangeEndingDate)
                );

                NotifyPropertyChanged("RetreivedRecordsLedger");
            }

            SelectFirstLedgerRecordIfAvailable();
        }

        void SelectFirstLedgerRecordIfAvailable()
        {
            if(RetreivedRecordsLedger.Count > 0)
            {
                RetreivedRecordsLedgerSelectedIndex = 0;
                PopulateSelectedPurchaseOrderBatchCollection();
            }
        }

        ObservableCollection<ReceivedPurchaseOrder> AggregateRecordsByPONumber(ObservableCollection<ReceivedBatch> results)
        {
            Dictionary<int, ReceivedPurchaseOrder> aggregatedBatches = new Dictionary<int, ReceivedPurchaseOrder>();

            foreach (ReceivedBatch batch in results)
            {
                if (aggregatedBatches.ContainsKey(batch.PONumber))
                {
                    aggregatedBatches[batch.PONumber].AddBatch(batch);
                }
                else
                {
                    ReceivedPurchaseOrder receivedPO = new ReceivedPurchaseOrder(
                        batch.PONumber, 
                        batch.ActivityDate, 
                        batch.ReceivingOperator
                    );

                    receivedPO.AddBatch(batch);
                    aggregatedBatches.Add(batch.PONumber, receivedPO);
                }
            }

            ObservableCollection<ReceivedPurchaseOrder> receivedPurchaseOrders = new ObservableCollection<ReceivedPurchaseOrder>();

            foreach (KeyValuePair<int, ReceivedPurchaseOrder> received in aggregatedBatches)
            {
                receivedPurchaseOrders.Add(received.Value);
            }

            return receivedPurchaseOrders;
        }

        public bool ReceivedPurchaseOrderIsSelected()
        {
            if (RetreivedRecordsLedger.Count > 0 && RetreivedRecordsLedgerSelectedIndex > -1)
            {
                return true;
            }

            SelectedPurchaseOrderReceivedBatches.Clear();
            NotifyPropertyChanged("SelectedPurchaseOrderReceivedBatches");

            return false;
        }

        public void PopulateSelectedPurchaseOrderBatchCollection()
        {
            SelectedPurchaseOrderReceivedBatches = RetreivedRecordsLedger[RetreivedRecordsLedgerSelectedIndex].ReceivedBatches;
            NotifyPropertyChanged("SelectedPurchaseOrderReceivedBatches");
        }
    }
}
