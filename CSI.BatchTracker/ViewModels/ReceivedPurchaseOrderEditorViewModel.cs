using CSI.BatchTracker.Domain;
using CSI.BatchTracker.Domain.Contracts;
using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Exceptions;
using CSI.BatchTracker.ViewModels.Commands;
using CSI.BatchTracker.Views.Contracts;
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

        public ObservableCollection<BatchOperator> BatchOperatorsList { get; private set; }
        public ICommand UpdatePurchaseOrderCommand { get; private set; }
        public ICommand UpdateReceivedBatchCommand { get; private set; }
        public ICommand DeleteReceivingRecordCommand { get; private set; }

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

        public DateTime ReceivingDate
        {
            get { return ReceivedBatch.ActivityDate; }
            set
            {
                ReceivedBatch.ActivityDate = value;
                NotifyPropertyChanged("ReceivingDate");
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

        public ObservableCollection<string> Colors
        {
            get { return colorList.Colors; }
        }

        int selectedColorIndex;
        public int SelectedColorIndex
        {
            get { return selectedColorIndex; }
            set
            {
                selectedColorIndex = value;
                ReceivedBatch.ColorName = Colors[selectedColorIndex];
                NotifyPropertyChanged("SelectedColorIndex");
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

        string updateText;
        public string UpdateText
        {
            get { return updateText; }
            set
            {
                updateText = value;
                NotifyPropertyChanged("UpdateText");
            }
        }

        ObservableCollection<ReceivedBatch> receivedBatches;
        public ObservableCollection<ReceivedBatch> ReceivedBatches
        {
            get { return receivedBatches; }
            private set
            {
                receivedBatches = value;
                NotifyPropertyChanged("ReceivedBatches");
            }
        }


        int receivedBatchesSelectedIndex;
        public int ReceivedBatchesSelectedIndex
        {
            get { return receivedBatchesSelectedIndex; }
            set
            {
                receivedBatchesSelectedIndex = value;

                if (receivedBatchesSelectedIndex > -1)
                {
                    ReceivedBatch = ReceivedBatches[receivedBatchesSelectedIndex];
                }
            }
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
            receivedBatches = new ObservableCollection<ReceivedBatch>();
            ReceivedBatch = new ReceivedBatch();
            ImportPurchaseOrderInformation();
            UpdatePurchaseOrderCommand = new UpdatePurchaseOrderHeaderCommand(this);
            DeleteReceivingRecordCommand = new DeletePurchaseOrderReceivingRecordCommand(this);
            UpdateText = "Save Item";
            this.colorList = colorList;
            this.batchNumberValidator = batchNumberValidator;
            SelectedColorIndex = 0;
            ReceivedBatchesSelectedIndex = -1;
        }

        void ImportPurchaseOrderInformation()
        {
            operatorSource.FindAllBatchOperators();
            BatchOperatorsList = operatorSource.OperatorRepository;
            PONumber = purchaseOrder.PONumber.ToString();
            ReceivingDate = purchaseOrder.ReceivingDate;
            ReceivedBatches = purchaseOrder.ReceivedBatches;
            SelectedOperatorIndex = FindSelectedOperatorIndexFromRepository();
            inventorySource.UpdateActiveInventory();
        }

        int FindSelectedOperatorIndexFromRepository()
        {
            int selected = -1;

            for (int i = 0; i < BatchOperatorsList.Count; i++)
            {
                if (ReceivedBatches.Count > 0 && BatchOperatorsList[i].FullName == ReceivedBatches[0].ReceivingOperator.FullName)
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
            int targetSystemId = purchaseOrder.GetReceivedBatchMappedSystemId(ReceivedBatchesSelectedIndex);
            receivedBatchSource.DeleteReceivedBatch(targetSystemId);
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
            inventorySource.UpdateActiveInventory();
            ReloadPurchaseOrder();
        }
    }
}
