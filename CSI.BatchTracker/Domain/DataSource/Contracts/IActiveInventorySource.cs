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
        void DeductBatchFromInventory(string batchNumber);
        ObservableCollection<InventoryBatch> CurrentInventory { get; }
        Dictionary<string, int> CurrentInventoryBatchNumberToIdMappings { get; }
    }
}
