namespace CSI.BatchTracker.Tests.Views
{
    class PassableIViewTestStub : IViewTestStub
    {
        public override bool CanShowView()
        {
            return true;
        }
    }
}
