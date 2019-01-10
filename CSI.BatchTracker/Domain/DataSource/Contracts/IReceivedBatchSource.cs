using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Domain.DataSource.Contracts
{
    public interface IReceivedBatchSource
    {
        ObservableCollection<ReceivedBatch> ReceivedBatchRepository { get; }
        Dictionary<int, int> ReceivedBatchIdMappings { get; }

        void SaveReceivedBatch(ReceivedBatch receivedBatch);
    }
}
