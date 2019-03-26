using CSI.BatchTracker.Views.Contracts;
using System.Windows;

namespace CSI.BatchTracker.Views
{
    abstract public class ViewBase : IView
    {
        protected Window window;

        public bool CanShowView()
        {
            if (window == null)
            {
                ResetWindow();
            }

            return window != null;
        }

        public void ShowView()
        {
            ResetWindow();
            window.ShowDialog();
        }

        abstract public void ResetWindow();
    }
}
