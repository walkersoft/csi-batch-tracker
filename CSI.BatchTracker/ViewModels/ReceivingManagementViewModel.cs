﻿using CSI.BatchTracker.Domain.Contracts;
using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.ViewModels.Commands;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CSI.BatchTracker.ViewModels
{
    public sealed class ReceivingManagementViewModel : ViewModelBase
    {
        IBatchNumberValidator batchNumberValidator;
        IColorList colorList;
        IBatchOperatorSource operatorSource;
        IReceivedBatchSource receivingSource;
        IActiveInventorySource inventorySource;

        public ICommand AddBatchToSessionLedgerCommand { get; private set; }
        public ICommand RemoveSelectedItemFromSessionLedgerCommand { get; private set; }
        public ICommand CommitSessionLedgerToReceivingLedgerCommand { get; private set; }

        public ReceivedBatch ReceivedBatch { get; private set; }
        public bool ColorListFocusState { get; set; }
        public int SessionLedgerSelectedIndex { get; set; }
        public ObservableCollection<ReceivedBatch> SessionLedger { get; private set; }
        public ObservableCollection<ReceivedBatch> ReceivedBatchRepository { get; private set; }
        public ObservableCollection<BatchOperator> BatchOperatorRepository { get; private set; }

        public DateTime ReceivingDate
        {
            get { return ReceivedBatch.ActivityDate; }
            set
            {
                ReceivedBatch.ActivityDate = value;
                NotifyPropertyChanged("ReceivingDate");
            }
        }

        int poNumber;
        string poNumberAsString;
        public string PONumber
        {
            get { return poNumberAsString; }
            set
            {
                if (int.TryParse(value, out poNumber))
                {
                    ReceivedBatch.PONumber = poNumber;
                }

                poNumberAsString = value;
                NotifyPropertyChanged("PONumber");
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
            get { return quantityAsString; }
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

        int receivingOperatorComboBoxIndex;
        public int ReceivingOperatorComboBoxIndex
        {
            get { return receivingOperatorComboBoxIndex; }
            set
            {
                receivingOperatorComboBoxIndex = value;
                NotifyPropertyChanged("ReceivingOperatorComboBoxIndex");
            }
        }

        int colorSelectionComboBoxIndex;
        public int ColorSelectionComboBoxIndex
        {
            get { return colorSelectionComboBoxIndex; }
            set
            {
                colorSelectionComboBoxIndex = value;
                NotifyPropertyChanged("ColorSelectionComboBoxIndex");
            }
        }

        public ObservableCollection<string> Colors
        {
            get { return colorList.Colors; }
        }

        string ColorName
        {
            get { return colorList.Colors[ColorSelectionComboBoxIndex]; }
        }

        BatchOperator ReceivingOperator
        {
            get
            {
                int targetId = operatorSource.BatchOperatorIdMappings[ReceivingOperatorComboBoxIndex];
                return operatorSource.FindBatchOperator(targetId);
            }
        }

        public ReceivingManagementViewModel(
            IBatchNumberValidator validator,
            IColorList colorList,
            IReceivedBatchSource receivingSource,
            IBatchOperatorSource operatorSource,
            IActiveInventorySource inventorySource)
        {
            this.receivingSource = receivingSource;
            this.operatorSource = operatorSource;
            this.inventorySource = inventorySource;
            this.colorList = colorList;
            batchNumberValidator = validator;

            this.operatorSource.FindAllBatchOperators();
            ReceivedBatchRepository = this.receivingSource.ReceivedBatchRepository;
            BatchOperatorRepository = this.operatorSource.OperatorRepository;
            SessionLedger = new ObservableCollection<ReceivedBatch>();
            SessionLedgerSelectedIndex = -1;
            ReceivedBatch = new ReceivedBatch();
            ReceivingDate = DateTime.Today;
            AddBatchToSessionLedgerCommand = new AddReceivedBatchToReceivingSessionLedgerCommand(this);
            RemoveSelectedItemFromSessionLedgerCommand = new RemoveReceivableBatchFromSessionLedgerCommand(this);
            CommitSessionLedgerToReceivingLedgerCommand = new CommitReceivingSessionLedgerToDataSourceCommand(this);
        }

        public bool ReceivedBatchIsValidForSessionLedger()
        {
            return string.IsNullOrEmpty(PONumber) == false
                && int.TryParse(PONumber, out poNumber)
                && ReceivingDate != DateTime.MinValue
                && ReceivingOperatorComboBoxIndex > -1
                && ColorSelectionComboBoxIndex > -1
                && batchNumberValidator.Validate(BatchNumber)
                && string.IsNullOrEmpty(Quantity) == false
                && int.TryParse(Quantity, out quantity)
                && quantity > 0
                && BatchIsConsistentWithCurrentReceiving()
                && BatchIsConsistentWithCurrentSession();
        }

        bool BatchIsConsistentWithCurrentReceiving()
        {
            ObservableCollection<ReceivedBatch> found = receivingSource.GetReceivedBatchesByBatchNumber(BatchNumber);
            string colorName = colorList.Colors[ColorSelectionComboBoxIndex].ToString();

            if (found.Count > 0 && colorName != found[0].ColorName)
            {
                return false;
            }

            return true;
        }

        bool BatchIsConsistentWithCurrentSession()
        {
            foreach (ReceivedBatch received in SessionLedger)
            {
                string colorName = colorList.Colors[ColorSelectionComboBoxIndex].ToString();

                if (received.BatchNumber == BatchNumber && received.ColorName != colorName)
                {
                    return false;
                }
            }

            return true;
        }

        public void AddReceivedBatchToSessionLedger()
        {
            if (ReceivedBatchIsValidForSessionLedger())
            {
                ReceivedBatch batch = new ReceivedBatch(
                    ColorName,
                    BatchNumber,
                    ReceivingDate,
                    quantity,
                    poNumber,
                    ReceivingOperator
                );

                SessionLedger.Add(batch);
                ResetLineItemData();
            }
        }

        void ResetLineItemData()
        {
            ColorSelectionComboBoxIndex = 0;
            BatchNumber = string.Empty;
            Quantity = string.Empty;
        }

        public bool SessionLedgerSelectedItemCanBeRemoved()
        {
            return SessionLedger.Count > 0
                && SessionLedgerSelectedIndex > -1;
        }

        public void RemoveSelectedEntryFromSessionLedger()
        {
            SessionLedger.RemoveAt(SessionLedgerSelectedIndex);
        }

        public void CommitSessionLedgerToDataSource()
        {
            EnsureSameReceivingOperatorOnAllBatches();

            foreach (ReceivedBatch batch in SessionLedger)
            {
                receivingSource.SaveReceivedBatch(batch);
            }

            ResetAllData();
        }

        void EnsureSameReceivingOperatorOnAllBatches()
        {
            foreach (ReceivedBatch batch in SessionLedger)
            {
                batch.ReceivingOperator = ReceivingOperator;
            }
        }

        void ResetAllData()
        {
            ReceivingDate = DateTime.Today;
            PONumber = string.Empty;
            SessionLedger.Clear();
            SessionLedgerSelectedIndex = -1;
            ReceivingOperatorComboBoxIndex = -1;
            ResetLineItemData();
        }

        public bool SessionLedgerCanBeCommited()
        {
            return SessionLedger.Count > 0;
        }
    }
}
