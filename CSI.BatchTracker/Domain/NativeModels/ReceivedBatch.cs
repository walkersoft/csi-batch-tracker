using CSI.BatchTracker.Exceptions;
using System;
using System.Text.RegularExpressions;

namespace CSI.BatchTracker.Domain.NativeModels
{
    public class ReceivedBatch : AbstractBatch
    {
        public int Quantity { get; set; }
        public int PONumber { get; set; }
        public BatchOperator ReceivingOperator { get; set; }

        public ReceivedBatch (
            string colorName, 
            string batchNumber, 
            DateTime receivingDate, 
            int quantity,
            int poNumber,
            BatchOperator receivingOperator
        )
        {
            // TODO: Move all of these to various validation classes.
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

        // TODO: Abstract to validation class.
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
