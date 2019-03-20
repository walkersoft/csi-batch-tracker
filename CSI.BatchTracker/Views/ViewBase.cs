using CSI.BatchTracker.Views.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
