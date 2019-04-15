namespace CSI.BatchTracker.Tests.Views
{
    class PassableIBatchHistoryViewerTestStub : IBatchHistoryViewerTestStub
    {
        public override bool CanShowView()
        {
            return true;
        }
    }
}
