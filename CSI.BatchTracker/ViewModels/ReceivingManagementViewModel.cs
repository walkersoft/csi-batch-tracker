using CSI.BatchTracker.Domain;
using CSI.BatchTracker.Domain.Contracts;
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
    public class ReceivingManagementViewModel
    {        
        public DateTime ReceivingDate { get; set; }
        public int ReceivingOperatorComboBoxIndex { get; set; }
        public int ColorSelectionComboBoxIndex { get; set; }

        public int SessionLedgerSelectedItem { get; set; }
        public string BatchNumber { get; set; }

        int poNumber;
        public string PONumber { get; set; }

        int quantity;
        public string Quantity { get; set; }

        public ReceivedBatch ReceivedBatch { get; private set; }
        public ObservableCollection<ReceivedBatch> SessionLedger { get; private set; }
        IBatchNumberValidator batchNumberValidator;
        IColorList colorList;
        IBatchOperatorSource operatorSource;

        public ReceivingManagementViewModel(IBatchNumberValidator validator, IColorList colorList, IBatchOperatorSource operatorSource)
        {
            batchNumberValidator = validator;
            this.colorList = colorList;
            this.operatorSource = operatorSource;
            SessionLedger = new ObservableCollection<ReceivedBatch>();
            SessionLedgerSelectedItem = -1;
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
                && SessionLedgerSelectedItem > -1;
        }

        public void RemoveSelectedEntryFromSessionLedger()
        {
            SessionLedger.RemoveAt(SessionLedgerSelectedItem);
        }
    }
}
