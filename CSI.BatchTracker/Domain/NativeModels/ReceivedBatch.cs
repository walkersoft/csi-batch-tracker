using CSI.BatchTracker.DataSource.Contracts;
using CSI.BatchTracker.Storage.Contracts;
using CSI.BatchTracker.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Domain.NativeModels
{
    public class ReceivedBatch : AbstractBatch
    {
        public int Quantity { get; private set; }
        public int PONumber { get; private set; }
        public BatchOperator ReceivingOperator { get; private set; }

        public ReceivedBatch (
            string colorName, 
            string batchNumber, 
            DateTime receivingDate, 
            int quantity,
            int poNumber,
            BatchOperator receivingOperator
        )
        {
            CheckIfColorNameIsEmpty(colorName);
            CheckIfBatchNumberIsEmpty(batchNumber);
            CheckIfQuantityIsGreaterThanZero(quantity);
            CheckIfPONumberIsAtLeastFiveDigits(poNumber);

            ColorName = colorName;
            BatchNumber = batchNumber;
            ActivityDate = receivingDate;
            Quantity = quantity;
            PONumber = poNumber;
            ReceivingOperator = receivingOperator;
        }

        void CheckIfPONumberIsAtLeastFiveDigits(int poNumber)
        {
            Match regex = Regex.Match(poNumber.ToString(), @"^[0-9]{5,}$");

            if (regex.Success == false)
            {
                throw new BatchException("PO number must be at least 5 digits.");
            }
        }
    }
}
