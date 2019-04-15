using CSI.BatchTracker.Views.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
