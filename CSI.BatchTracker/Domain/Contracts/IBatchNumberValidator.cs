namespace CSI.BatchTracker.Domain.Contracts
{
    public interface IBatchNumberValidator
    {
        bool Validate(string batchNumber);
        int GetBatchNumberLength();
    }
}
