﻿using CSI.BatchTracker.DataSource.Contracts;
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
    public class ReceivingManagementViewModel : ViewModelBase
    {
        public ICommand AddBatchToSessionLedgerCommand { get; private set; }
        public ICommand RemoveSelectedItemFromSessionLedgerCommand { get; private set; }
        public ICommand CommitSessionLedgerToReceivingLedgerCommand { get; private set; }
        public ReceivedBatch ReceivedBatch { get; private set; }

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
                if(int.TryParse(value, out quantity))
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

        public int SessionLedgerSelectedIndex { get; set; }
        public ObservableCollection<ReceivedBatch> SessionLedger { get; private set; }
        public ObservableCollection<ReceivedBatch> ReceivedBatchRepository { get; private set; }
        public ObservableCollection<BatchOperator> BatchOperatorRepository { get; private set; }

        IBatchNumberValidator batchNumberValidator;
        IColorList colorList;
        IBatchOperatorSource operatorSource;
        IReceivedBatchSource receivingSource;

        public ReceivingManagementViewModel(
            IBatchNumberValidator validator, 
            IColorList colorList, 
            IReceivedBatchSource receivingSource,
            IBatchOperatorSource operatorSource)
        {
            this.receivingSource = receivingSource;
            this.operatorSource = operatorSource;
            ReceivedBatchRepository = this.receivingSource.ReceivedBatchRepository;
            BatchOperatorRepository = this.operatorSource.OperatorRepository;
            operatorSource.FindAllBatchOperators();
            batchNumberValidator = validator;
            this.colorList = colorList;
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
                && quantity > 0;
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
            ColorSelectionComboBoxIndex = -1;
            BatchNumber = string.Empty;
            Quantity = string.Empty;
        }

        string ColorName
        {
            get
            {
                return colorList.Colors[ColorSelectionComboBoxIndex];
            }
        }

        BatchOperator ReceivingOperator
        {
            get
            {
                int targetId = operatorSource.BatchOperatorIdMappings[ReceivingOperatorComboBoxIndex];
                return operatorSource.FindBatchOperator(targetId);
            }
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
            foreach (ReceivedBatch batch in SessionLedger)
            {
                receivingSource.SaveReceivedBatch(batch);
            }

            ResetAllData();
        }

        void ResetAllData()
        {
            ReceivingDate = DateTime.MinValue;
            PONumber = string.Empty;
            ReceivingOperatorComboBoxIndex = -1;
            ResetLineItemData();
        }
    }
}
