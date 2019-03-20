using CSI.BatchTracker.Views.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
