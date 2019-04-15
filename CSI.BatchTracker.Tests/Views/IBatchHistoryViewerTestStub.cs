using CSI.BatchTracker.Views.Contracts;

namespace CSI.BatchTracker.Tests.Views
{
    public class IBatchHistoryViewerTestStub : IBatchHistoryView
    {
        public string IncomingBatchNumber { get; set; }

        public virtual bool CanShowView()
        {
            return false;
        }

        public void ShowView()
        {
            return;
        }
    }
}
