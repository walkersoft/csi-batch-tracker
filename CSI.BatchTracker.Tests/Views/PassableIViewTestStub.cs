namespace CSI.BatchTracker.Tests.Views
{
    internal class PassableIViewTestStub : IViewTestStub
    {
        public override bool CanShowView()
        {
            return true;
        }
    }
}
