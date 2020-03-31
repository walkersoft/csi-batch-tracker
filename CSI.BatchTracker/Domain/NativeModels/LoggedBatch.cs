using System;

namespace CSI.BatchTracker.Domain.NativeModels
{
    public sealed class LoggedBatch : AbstractBatch
    {
        public BatchOperator ImplementingOperator { get; private set; }

        public LoggedBatch(string colorName, string batchNumber, DateTime implementationDate, BatchOperator implementationOperator)
        {
            CheckIfColorNameIsEmpty(colorName);
            CheckIfBatchNumberIsEmpty(batchNumber);

            ColorName = colorName;
            BatchNumber = batchNumber;
            ActivityDate = implementationDate;
            ImplementingOperator = implementationOperator;
        }
    }
}
