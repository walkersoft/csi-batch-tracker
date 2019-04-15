namespace CSI.BatchTracker.Views.Contracts
{
    public interface IBatchHistoryView : IView
    {
        string IncomingBatchNumber { get; set; }
    }
}
