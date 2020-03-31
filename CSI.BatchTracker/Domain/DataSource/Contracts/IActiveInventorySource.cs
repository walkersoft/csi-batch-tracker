using CSI.BatchTracker.Domain.NativeModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CSI.BatchTracker.Domain.DataSource.Contracts
{
    public interface IActiveInventorySource
    {
        ObservableCollection<InventoryBatch> CurrentInventory { get; }
        Dictionary<string, int> CurrentInventoryBatchNumberToIdMappings { get; }
        int TotalInventoryCount { get; }

        void UpdateActiveInventory();
        void AddReceivedBatchToInventory(ReceivedBatch batch);
        void DeductBatchFromInventory(string batchNumber);
        InventoryBatch FindInventoryBatchByBatchNumber(string batchNumber);
    }
}
