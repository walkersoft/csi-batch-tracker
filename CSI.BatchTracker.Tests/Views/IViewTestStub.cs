using CSI.BatchTracker.Views.Contracts;

namespace CSI.BatchTracker.Tests.Views
{
    public class IViewTestStub : IView
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
