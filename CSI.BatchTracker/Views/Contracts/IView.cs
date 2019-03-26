namespace CSI.BatchTracker.Views.Contracts
{
    public interface IView
    {
        bool CanShowView();
        void ShowView();
    }
}
