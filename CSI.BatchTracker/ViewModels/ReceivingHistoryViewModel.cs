using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.ViewModels.Commands;
using CSI.BatchTracker.Views;
using CSI.BatchTracker.Views.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

namespace CSI.BatchTracker.ViewModels
{
    public sealed class ReceivingHistoryViewModel : ViewModelBase
    {
        IReceivedBatchSource receivedBatchSource;
        IActiveInventorySource inventorySource;
        ReceivingManagementViewModel receivingManagementViewModel;

        public SearchCriteriaVisibilityManager VisibilityManager { get; set; }
        public DateTime DateRangeStartingDate { get; set; }
        public DateTime DateRangeEndingDate { get; set; }
        public ObservableCollection<ReceivedBatch> SelectedPurchaseOrderReceivedBatches { get; set; }
        public int RetreivedRecordsLedgerSelectedIndex { get; set; }
        public int DatePeriodSelectedIndex { get; set; }
        public DateTime SpecificDate { get; set; }
        public IView ReceivingSessionViewer { get; set; }

        public ICommand ListBatchesFromReceivedPurchaseOrder { get; private set; }
        public ICommand ChangeSearchCriteriaPanelVisibility { get; private set;}
        public ICommand PopulateRetreivedRecordsLedgerFromSearchCriteria { get; private set; }
        public ICommand OpenReceivedBatchSetForViewing { get; private set; }

        int poNumber;
        string poNumberAsString;
        public string SpecificPONumber
        {
            get { return poNumberAsString; }
            set
            {
                if (int.TryParse(value, out poNumber))
                {
                    poNumberAsString = poNumber.ToString();
                }

                NotifyPropertyChanged("PONumber");
                SelectFirstLedgerRecordIfAvailable();
            }
        }

        int searchCriteriaSelectedIndex;
        public int SearchCriteriaSelectedIndex
        {
            get { return searchCriteriaSelectedIndex; }
            set
            {
                searchCriteriaSelectedIndex = value;
                SetActiveSearchCritera();
            }
        }

        ObservableCollection<ReceivedPurchaseOrder> retreivedRecordsLedger;
        public ObservableCollection<ReceivedPurchaseOrder> RetreivedRecordsLedger
        {
            get { return retreivedRecordsLedger; }
            set
            {
                retreivedRecordsLedger = value;
                NotifyPropertyChanged("RetreivedRecordsLedger");
                SelectFirstLedgerRecordIfAvailable();
            }
        }

        public ReceivingHistoryViewModel(
            IReceivedBatchSource receivedBatchSource,
            IActiveInventorySource inventorySource,
            ReceivingManagementViewModel viewModel)
        {
            this.receivedBatchSource = receivedBatchSource;
            this.inventorySource = inventorySource;
            receivingManagementViewModel = viewModel;
            VisibilityManager = new SearchCriteriaVisibilityManager();
            SelectedPurchaseOrderReceivedBatches = new ObservableCollection<ReceivedBatch>();
            RetreivedRecordsLedger = new ObservableCollection<ReceivedPurchaseOrder>();
            SearchCriteriaSelectedIndex = 0;
            DatePeriodSelectedIndex = 0;
            DateRangeStartingDate = DateTime.Today;
            DateRangeEndingDate = DateTime.Today;
            SpecificDate = DateTime.Today;
            PopulateRetreivedRecordsLedgerFromSearchCriteria = new ListReceivingRecordsByDateRangeCommand(this);
            ListBatchesFromReceivedPurchaseOrder = new ListBatchesFromReceivedPurchaseOrderCommand(this);
            ChangeSearchCriteriaPanelVisibility = new ChangeSearchCriteriaPanelVisibilityCommand(this);
            OpenReceivedBatchSetForViewing = new OpenReceivingRecordForViewingCommand(this);
            ReceivingSessionViewer = new BatchReceivingManagementViewer(receivingManagementViewModel);
        }

        public bool DateRangeCriteriaIsMet()
        {
            return DateRangeStartingDate != null
                && DateRangeEndingDate != null
                && DateRangeStartingDateIsOnOrBeforeEndingDate();
        }

        bool DateRangeStartingDateIsOnOrBeforeEndingDate()
        {
            return DateRangeStartingDate.Date <= DateRangeEndingDate.Date;
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

        public bool ReceivedPurchaseOrderIsSelectedAndReceivingSessionIsReady()
        {
            return ReceivedPurchaseOrderIsSelected()
                && ReceivingSessionViewer != null
                && ReceivingSessionViewer.CanShowView();
        }

        public void OpenReceivingSessionToViewSelectedPurchaseOrder()
        {
            receivingManagementViewModel.EditModeEnabled = false;
            receivingManagementViewModel.PopulateFromPONumber(RetreivedRecordsLedger[RetreivedRecordsLedgerSelectedIndex].PONumber);
            ReceivingSessionViewer.ShowView();
        }

        public void PopulateSelectedPurchaseOrderBatchCollection()
        {
            SelectedPurchaseOrderReceivedBatches = RetreivedRecordsLedger[RetreivedRecordsLedgerSelectedIndex].ReceivedBatches;
            NotifyPropertyChanged("SelectedPurchaseOrderReceivedBatches");
        }

        public bool SearchCriteriaVisibilityManagerIsSet()
        {
            return VisibilityManager != null;
        }

        public void SetActiveSearchCritera()
        {
            if (SearchCriteriaSelectedIndex == 0)
            {
                PopulateRetreivedRecordsLedgerFromSearchCriteria = new ListReceivingRecordsByDateRangeCommand(this);
                VisibilityManager.SetVisibility(SearchCriteriaVisibilityManager.ActiveCriteriaPanel.DateRange);
            }

            if (SearchCriteriaSelectedIndex == 1)
            {
                PopulateRetreivedRecordsLedgerFromSearchCriteria = new ListReceivingRecordsByDatePeriodCommand(this);
                VisibilityManager.SetVisibility(SearchCriteriaVisibilityManager.ActiveCriteriaPanel.DatePeriod);
            }

            if (SearchCriteriaSelectedIndex == 2)
            {
                PopulateRetreivedRecordsLedgerFromSearchCriteria = new ListReceivingRecordsBySpecificDateCommand(this);
                VisibilityManager.SetVisibility(SearchCriteriaVisibilityManager.ActiveCriteriaPanel.SpecificDate);
            }

            if (SearchCriteriaSelectedIndex == 3)
            {
                PopulateRetreivedRecordsLedgerFromSearchCriteria = new ListReceivingRecordsByPONumberCommand(this);
                VisibilityManager.SetVisibility(SearchCriteriaVisibilityManager.ActiveCriteriaPanel.PONumber);
            }

            NotifyPropertyChanged("PopulateRetreivedRecordsLedgerFromSearchCriteria");
        }

        public bool PONumberIsValid()
        {
            return string.IsNullOrEmpty(SpecificPONumber) == false
                && int.TryParse(SpecificPONumber, out poNumber);
        }

        public bool SpecifcDateCriteriaIsSet()
        {
            return SpecificDate > DateTime.MinValue;
        }

        void SelectFirstLedgerRecordIfAvailable()
        {
            if (RetreivedRecordsLedger.Count > 0)
            {
                RetreivedRecordsLedgerSelectedIndex = 0;
                PopulateSelectedPurchaseOrderBatchCollection();

                return;
            }

            SelectedPurchaseOrderReceivedBatches.Clear();
        }

        public void FetchReceivingRecordsByDateRange()
        {
            RetreivedRecordsLedger = AggregateRecordsByPONumber(
                receivedBatchSource.GetReceivedBatchesWithinDateRange(DateRangeStartingDate, DateRangeEndingDate)
            );
        }

        public void FetchReceivingRecordsByPONumber()
        {
            RetreivedRecordsLedger = AggregateRecordsByPONumber(
                receivedBatchSource.GetReceivedBatchesByPONumber(poNumber)
            );
        }

        public void FetchReceivingRecordsBySpecificDate()
        {
            RetreivedRecordsLedger = AggregateRecordsByPONumber(
                receivedBatchSource.GetReceivedBatchesbySpecificDate(SpecificDate)
            );
        }

        public void FetchReceivingRecordsByDatePeriod()
        {
            DateTime startingDate = DateTime.Today;
            DateTime endingDate = DateTime.Today;

            if (DatePeriodSelectedIndex == 0)
            {
                startingDate = endingDate.AddDays(-30);
            }

            if (DatePeriodSelectedIndex == 1)
            {
                startingDate = endingDate.AddDays(-90);
            }

            if (DatePeriodSelectedIndex == 2)
            {
                startingDate = endingDate.AddDays(-365);
            }

            RetreivedRecordsLedger = AggregateRecordsByPONumber(
                receivedBatchSource.GetReceivedBatchesWithinDateRange(startingDate, endingDate)
            );
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
    }
}
