using CSI.BatchTracker.Exceptions;
using System;

namespace CSI.BatchTracker.Domain.NativeModels
{
    abstract public class AbstractBatch
    {
        public string ColorName { get; protected set; }
        public string BatchNumber { get; protected set; }
        public DateTime ActivityDate { get; protected set; }

        protected void CheckIfQuantityIsGreaterThanZero(int amount)
        {
            if (amount < 1)
            {
                throw new BatchException("Quantity given must be greater than zero.");
            }
        }

        protected void CheckIfColorNameIsEmpty(string colorName)
        {
            if (CheckIfStringIsEmpty(colorName))
            {
                throw new BatchException("Color name cannot be empty.");
            }
        }

        protected void CheckIfBatchNumberIsEmpty(string batchNumber)
        {
            if (CheckIfStringIsEmpty(batchNumber))
            {
                throw new BatchException("Batch number cannot be empty.");
            }
        }

        bool CheckIfStringIsEmpty(string text)
        {
            return text.Length == 0;
        }
    }
}
