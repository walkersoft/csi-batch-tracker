using CSI.BatchTracker.Domain.Contracts;
using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.ViewModels.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CSI.BatchTracker.ViewModels
{
    public sealed class BatchHistoryViewModel : ViewModelBase
    {
        
        string batchNumber;
        public string BatchNumber
        {
            get { return batchNumber; }
            set
            {
                batchNumber = value;
                NotifyPropertyChanged("BatchNumber");
            }
        }

        string amountInInventory;
        public string AmountInInventory
        {
            get { return amountInInventory; }
            set
            {
                amountInInventory = value;
                NotifyPropertyChanged("BatchNumber");
            }
        }

        public string RetrievedBatchNumber { get; private set; }

        public ObservableCollection<ReceivedBatch> ReceivingHistoryGrid { get; private set; }
        public ObservableCollection<LoggedBatch> ImplementationHistoryGrid { get; private set; }
        public ICommand RetrieveBatchDataHistoryCommand { get; private set; }

        IBatchNumberValidator validator;
        IActiveInventorySource inventorySource;
        IReceivedBatchSource receivedBatchSource;
        IImplementedBatchSource implementedBatchSource;

        public BatchHistoryViewModel(
            IBatchNumberValidator validator,
            IActiveInventorySource inventorySource,
            IReceivedBatchSource receivedBatchSource,
            IImplementedBatchSource implementedBatchSource)
        {
            this.validator = validator;
            this.inventorySource = inventorySource;
            this.receivedBatchSource = receivedBatchSource;
            this.implementedBatchSource = implementedBatchSource;
            AmountInInventory = "0";
            BatchNumber = string.Empty;
            RetrieveBatchDataHistoryCommand = new DisplayBatchHistoryFromBatchNumberCommand(this);
        }

        public bool BatchNumberIsValid()
        {
            return validator.Validate(BatchNumber);
        }

        public void RetrieveBatchHistoryData()
        {
            GetCurrentInventoryAmount();
            GetReceivingHistoryData();
            GetImplementationHistoryData();
            RetrievedBatchNumber = BatchNumber;
            NotifyPropertyChanged("RetrievedBatchNumber");
        }

        void GetCurrentInventoryAmount()
        {
            InventoryBatch batch = inventorySource.FindInventoryBatchByBatchNumber(BatchNumber);
            AmountInInventory = batch.Quantity.ToString();
            NotifyPropertyChanged("AmountInInventory");
        }

        void GetReceivingHistoryData()
        {
            receivedBatchSource.FindReceivedBatchesByBatchNumber(BatchNumber);
            ReceivingHistoryGrid = receivedBatchSource.ReceivedBatchRepository;
            NotifyPropertyChanged("ReceivingHistoryGrid");
        }

        void GetImplementationHistoryData()
        {
            implementedBatchSource.FindImplementedBatchesByBatchNumber(BatchNumber);
            ImplementationHistoryGrid = implementedBatchSource.ImplementedBatchLedger;
            NotifyPropertyChanged("ImplementationHistoryGrid");
        }
    }
}
