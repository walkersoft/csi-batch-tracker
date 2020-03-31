namespace CSI.BatchTracker.Tests.Views
{
    internal class PassableIBatchHistoryViewerTestStub : IBatchHistoryViewerTestStub
    {
        public override bool CanShowView()
        {
            return true;
        }
    }
}
