using CSI.BatchTracker.Domain;
using CSI.BatchTracker.Domain.Contracts;
using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.ViewModels.Commands;
using System;
using System.Collections.ObjectModel;
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
        public ICommand ReceivedBatchSelectionChanged { get; private set; }

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

        int quantity, originalQuantity;
        string quantityAsString;
        public string Quantity
        {
            get { return ReceivedBatch.Quantity.ToString(); }
            set
            {
                if (int.TryParse(value, out quantity) == false)
                {
                    quantity = 0;
                }

                ReceivedBatch.Quantity = quantity;
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
                    ReceivedBatch selectedBatch = new ReceivedBatch(
                        ReceivedBatches[receivedBatchesSelectedIndex].ColorName,
                        ReceivedBatches[receivedBatchesSelectedIndex].BatchNumber,
                        ReceivedBatches[receivedBatchesSelectedIndex].ActivityDate,
                        ReceivedBatches[receivedBatchesSelectedIndex].Quantity,
                        ReceivedBatches[receivedBatchesSelectedIndex].PONumber,
                        ReceivedBatches[receivedBatchesSelectedIndex].ReceivingOperator
                    );

                    ReceivedBatch = selectedBatch;
                    originalQuantity = ReceivedBatch.Quantity;
                }
            }
        }

        ReceivedBatch receivedBatch;
        ReceivedBatch ReceivedBatch
        {
            get { return receivedBatch; }
            set
            {
                receivedBatch = value;
                NotifyPropertyChanged("ReceivedBatch");
                NotifyPropertyChanged("SelectedColorIndex");
                NotifyPropertyChanged("BatchNumber");
                NotifyPropertyChanged("Quantity");
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
            UpdateReceivedBatchCommand = new UpdatePurchaseOrderReceivingRecordCommand(this);
            UpdatePurchaseOrderCommand = new UpdatePurchaseOrderHeaderCommand(this);
            DeleteReceivingRecordCommand = new DeletePurchaseOrderReceivingRecordCommand(this);
            ReceivedBatchSelectionChanged = new ReceivedBatchForEditingSelectionChangedCommand(this);
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
            ResetSelectedPurchaseOrder();
        }

        void ResetSelectedPurchaseOrder()
        {
            ReceivedBatchesSelectedIndex = -1;
            SelectedColorIndex = 0;
            BatchNumber = string.Empty;
            Quantity = "0";
        }

        public bool SelectedReceivingRecordIsReadyForUpdate()
        {
            return ReceivedBatchesSelectedIndex > -1
                && SelectedColorIndex > -1
                && batchNumberValidator.Validate(ReceivedBatch.BatchNumber)
                && ReceivedBatch.Quantity > 0
                && QuantityAvailableForReduction(ReceivedBatches[ReceivedBatchesSelectedIndex], ReceivedBatch.Quantity)
                && SelectedRecordIsConsistentWithBatchAndColorName();
        }

        bool QuantityAvailableForReduction(ReceivedBatch originalBatch, int editedQuantity)
        {
            ObservableCollection<ReceivedBatch> received = receivedBatchSource.GetReceivedBatchesByBatchNumber(originalBatch.BatchNumber);
            int receivedSum = 0;

            foreach (ReceivedBatch batch in received)
            {
                receivedSum += batch.Quantity;
            }

            ObservableCollection<LoggedBatch> logged = implementedBatchSource.GetImplementedBatchesByBatchNumber(originalBatch.BatchNumber);
            int difference = editedQuantity - originalQuantity;

            return logged.Count <= receivedSum + difference;
        }

        bool SelectedRecordIsConsistentWithBatchAndColorName()
        {
            ObservableCollection<ReceivedBatch> found = receivedBatchSource.GetReceivedBatchesByBatchNumber(BatchNumber);
            string colorName = Colors[SelectedColorIndex].ToString();

            if (found.Count > 0 && colorName != found[0].ColorName)
            {
                return false;
            }

            return true;
        }

        public void UpdateSelectedReceivingRecord()
        {
            int systemId = purchaseOrder.GetReceivedBatchMappedSystemId(ReceivedBatchesSelectedIndex);
            receivedBatchSource.UpdateReceivedBatch(systemId, ReceivedBatch);
            inventorySource.UpdateActiveInventory();
            ReloadPurchaseOrder();
        }

        public bool ReceivedRecordIsSelected()
        {
            return ReceivedBatchesSelectedIndex > -1;
        }

        public void PopulateSelectedReceivedRecord()
        {
            ReceivedBatch selectedBatch = GetReceivedBatchFromSelectedRecordAtIndex(ReceivedBatchesSelectedIndex);
            ReceivedBatch.ColorName = selectedBatch.ColorName;
            BatchNumber = selectedBatch.BatchNumber;
            Quantity = selectedBatch.Quantity.ToString();
            SetSelectedColorIndex(ReceivedBatch.ColorName);
            UpdateText = "Update Item";
        }

        ReceivedBatch GetReceivedBatchFromSelectedRecordAtIndex(int index)
        {
            return new ReceivedBatch(
                ReceivedBatches[index].ColorName,
                ReceivedBatches[index].BatchNumber,
                ReceivedBatches[index].ActivityDate,
                ReceivedBatches[index].Quantity,
                ReceivedBatches[index].PONumber,
                ReceivedBatches[index].ReceivingOperator
            );
        }

        void SetSelectedColorIndex(string colorName)
        {
            for (int i = 0; i < Colors.Count; i++)
            {
                if (Colors[i].ToString() == colorName)
                {
                    SelectedColorIndex = i;
                }
            }
        }
    }
}
