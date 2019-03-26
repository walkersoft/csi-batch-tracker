using CSI.BatchTracker.Domain.NativeModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CSI.BatchTracker.Domain.DataSource.Contracts
{
    public interface IActiveInventorySource
    {
        void AddReceivedBatchToInventory(ReceivedBatch batch);
        InventoryBatch FindInventoryBatchByBatchNumber(string batchNumber);
        void DeductBatchFromInventory(string batchNumber);
        void UpdateActiveInventory();
        ObservableCollection<InventoryBatch> CurrentInventory { get; }
        Dictionary<string, int> CurrentInventoryBatchNumberToIdMappings { get; }
    }
}
