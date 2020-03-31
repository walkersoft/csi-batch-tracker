using CSI.BatchTracker.Views.Contracts;

namespace CSI.BatchTracker.Tests.Views
{
    internal class IViewTestStub : IView
    {
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
