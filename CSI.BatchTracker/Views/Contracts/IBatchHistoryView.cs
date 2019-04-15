using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Views.Contracts
{
    public interface IBatchHistoryView : IView
    {
        string IncomingBatchNumber { get; set; }
    }
}
