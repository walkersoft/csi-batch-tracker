using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
