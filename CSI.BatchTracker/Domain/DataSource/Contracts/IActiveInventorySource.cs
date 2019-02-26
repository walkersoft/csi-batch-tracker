using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Domain.DataSource.Contracts
{
    public interface IActiveInventorySource
    {
        void AddReceivedBatchToInventory(ReceivedBatch batch);
        InventoryBatch FindInventoryBatchByBatchNumber(string batchNumber);
        ObservableCollection<InventoryBatch> CurrentInventory { get; }
        Dictionary<int, string> CurrentInventoryBatchNumberMappings { get; }
    }
}
