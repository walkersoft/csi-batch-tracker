using CSI.BatchTracker.Domain;
using CSI.BatchTracker.Domain.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
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
        public string BatchNumber { get; set; }

        int poNumber;
        public string PONumber { get; set; }

        int quantity;
        public string Quantity { get; set; }

        public ReceivedBatch ReceivedBatch { get; private set; }
        IBatchNumberValidator batchNumberValidator;

        public ReceivingManagementViewModel(IBatchNumberValidator validator)
        {
            batchNumberValidator = validator;
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
    }
}
