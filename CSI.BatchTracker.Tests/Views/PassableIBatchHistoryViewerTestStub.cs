using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.Views
{
    class PassableIBatchHistoryViewerTestStub : IBatchHistoryViewerTestStub
    {
        public override bool CanShowView()
        {
            return true;
        }
    }
}
