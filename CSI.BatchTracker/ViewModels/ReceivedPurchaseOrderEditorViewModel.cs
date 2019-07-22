using CSI.BatchTracker.Domain;
using CSI.BatchTracker.Domain.Contracts;
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
    public sealed class ReceivedPurchaseOrderEditorViewModel : ViewModelBase
    {
        IBatchOperatorSource operatorSource;
        IActiveInventorySource inventorySource;
        IReceivedBatchSource receivedBatchSource;
        IImplementedBatchSource implementedBatchSource;
        IColorList colorList;
        IBatchNumberValidator batchNumberValidator;
        EditablePurchaseOrder purchaseOrder;

        public ObservableCollection<ReceivedBatch> ReceivedBatches { get; private set; }
        public ObservableCollection<BatchOperator> BatchOperatorsList { get; private set; }
        public string UpdateText { get; set; }
        public int SelectedColorIndex { get; set; }
        public ObservableCollection<ReceivedBatch> PurchaseOrderLedger { get; set; }
        public int ReceivedBatchesSelectedIndex { get; set; }
        public ICommand UpdatePurchaseOrderCommand { get; private set; }
        public ICommand UpdateReceivedBatchCommand { get; private set; }
        public ICommand DeleteReceivingRecordCommand { get; private set; }

        public ObservableCollection<string> Colors
        {
            get { return colorList.Colors; }
        }

        ReceivedBatch receivedBatch;
        public ReceivedBatch ReceivedBatch
        {
            get { return receivedBatch; }
            set
            {
                receivedBatch = value;
                NotifyPropertyChanged("ReceivedBatch");
            }
        }

        int poNumberAsInt;
        string poNumberAsString;
        public string PONumber
        {
            get { return poNumberAsString; }
            set
            {
                if (int.TryParse(value, out poNumberAsInt))
                {
                    poNumberAsString = value;
                    NotifyPropertyChanged("PONumber");
                }
            }
        }

        DateTime receivingDate;
        public DateTime ReceivingDate
        {
            get { return ReceivedBatch.ActivityDate; }
            set
            {
                ReceivedBatch.ActivityDate = value;
                NotifyPropertyChanged("ReceivingDate");
            }
        }

        public string BatchNumber
        {
            get { return ReceivedBatch.BatchNumber; }
            set
            {
                ReceivedBatch.BatchNumber = value;
                NotifyPropertyChanged("BatchNumber");
            }
        }

        int quantity;
        string quantityAsString;
        public string Quantity
        {
            get { return ReceivedBatch.Quantity.ToString(); }
            set
            {
                if (int.TryParse(value, out quantity))
                {
                    ReceivedBatch.Quantity = quantity;
                }

                quantityAsString = value;
                NotifyPropertyChanged("Quantity");
            }
        }

        int selectedOperatorIndex;
        public int SelectedOperatorIndex
        {
            get { return selectedOperatorIndex; }
            set
            {
                selectedOperatorIndex = value;
                NotifyPropertyChanged("SelectedOperatorIndex");
            }
        }

        public ReceivedPurchaseOrderEditorViewModel(
            EditablePurchaseOrder purchaseOrder,
            IColorList colorList,
            IBatchNumberValidator batchNumberValidator,
            IBatchOperatorSource operatorSource,
            IActiveInventorySource inventorySource,
            IReceivedBatchSource receivedBatchSource,
            IImplementedBatchSource implementedBatchSource
        )
        {
            this.purchaseOrder = purchaseOrder;
            this.operatorSource = operatorSource;
            this.inventorySource = inventorySource;
            this.receivedBatchSource = receivedBatchSource;
            this.implementedBatchSource = implementedBatchSource;
            ReceivedBatch = new ReceivedBatch();
            ImportPurchaseOrderInformation();
            UpdatePurchaseOrderCommand = new UpdatePurchaseOrderHeaderCommand(this);
            UpdateText = "Save Item";
            this.colorList = colorList;
            this.batchNumberValidator = batchNumberValidator;
        }

        void ImportPurchaseOrderInformation()
        {
            operatorSource.FindAllBatchOperators();
            BatchOperatorsList = operatorSource.OperatorRepository;
            PONumber = purchaseOrder.PONumber.ToString();
            ReceivingDate = purchaseOrder.ReceivingDate;
            ReceivedBatches = purchaseOrder.ReceivedBatches;
            SelectedOperatorIndex = FindSelectedOperatorIndexFromRepository();
        }

        int FindSelectedOperatorIndexFromRepository()
        {
            int selected = -1;

            for (int i = 0; i < BatchOperatorsList.Count; i++)
            {
                if (BatchOperatorsList[i].FullName == ReceivedBatches[0].ReceivingOperator.FullName)
                {
                    selected = i;
                }
            }

            return selected;
        }

        public bool AllHeaderFieldsArePopulated()
        {
            return string.IsNullOrEmpty(PONumber) == false
                && poNumberAsInt > 0
                && ReceivingDate > DateTime.MinValue
                && SelectedOperatorIndex > -1;
        }

        public void UpdatePurchaseOrderHeaderInformation()
        {
            for (int i = 0; i < ReceivedBatches.Count; i++)
            {
                ReceivedBatches[i].PONumber = poNumberAsInt;
                ReceivedBatches[i].ActivityDate = ReceivingDate;
                ReceivedBatches[i].ReceivingOperator = operatorSource.FindBatchOperator(operatorSource.BatchOperatorIdMappings[SelectedOperatorIndex]);
                int systemId = purchaseOrder.GetReceivedBatchMappedSystemId(i);
                receivedBatchSource.UpdateReceivedBatch(systemId, ReceivedBatches[i]);
            }

            ReloadPurchaseOrder();
        }

        void ReloadPurchaseOrder()
        {
            purchaseOrder = receivedBatchSource.GetPurchaseOrderForEditing(poNumberAsInt);
            ImportPurchaseOrderInformation();
        }

        public bool SelectedReceivedBatchHasNotBeenImplemented()
        {
            return ReceivedBatchesSelectedIndex > -1
                && implementedBatchSource.GetImplementedBatchesByBatchNumber(ReceivedBatches[ReceivedBatchesSelectedIndex].BatchNumber).Count == 0;
        }

        public void DeleteSelectedReceivingRecordFromLedger()
        {
            receivedBatchSource.DeleteReceivedBatch(purchaseOrder.GetReceivedBatchMappedSystemId(ReceivedBatchesSelectedIndex));
            ReloadPurchaseOrder();
        }

        public bool SelectedReceivingRecordIsReadyForUpdate()
        {
            return ReceivedBatchesSelectedIndex > -1
                && SelectedColorIndex > -1
                && batchNumberValidator.Validate(ReceivedBatch.BatchNumber)
                && ReceivedBatch.Quantity > 0
                && ReceivedBatch.Quantity >= implementedBatchSource.GetImplementedBatchesByBatchNumber(ReceivedBatches[ReceivedBatchesSelectedIndex].BatchNumber).Count;
        }

        public void UpdateSelectedReceivingRecord()
        {
            int systemId = purchaseOrder.GetReceivedBatchMappedSystemId(ReceivedBatchesSelectedIndex);
            receivedBatchSource.UpdateReceivedBatch(systemId, ReceivedBatch);
            ReloadPurchaseOrder();
        }
    }
}
