namespace CSI.BatchTracker.Domain.Contracts
{
    abstract public class AbstractBatchNumberValidator : IBatchNumberValidator
    {
        public int BatchNumberLength { get; protected set; }

        abstract public bool Validate(string batchNumber);

        public int GetBatchNumberLength()
        {
            return BatchNumberLength;
        }
    }
}
