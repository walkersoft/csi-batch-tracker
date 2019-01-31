using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.ObjectModel;

namespace CSI.BatchTracker.DataSource.Contracts
{
    public interface IDataSource : IBatchOperatorSource, IReceivedBatchSource
    {
        ObservableCollection<InventoryBatch> InventoryRepository { get; }
        ObservableCollection<LoggedBatch> BatchLedger { get; }
        void ReceiveInventory(ReceivedBatch batch);
        void ImplementBatch(string batchNumber, DateTime implementationDate, BatchOperator batchOperator);
    }
}
