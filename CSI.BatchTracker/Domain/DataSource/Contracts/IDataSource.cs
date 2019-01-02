using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.DataSource.Contracts
{
    public interface IDataSource : IBatchOperatorSource
    {
        ObservableCollection<InventoryBatch> InventoryRepository { get; }
        ObservableCollection<LoggedBatch> BatchLedger { get; }
        void ReceiveInventory(ReceivedBatch batch);
        void ImplementBatch(string batchNumber, DateTime implementationDate, BatchOperator batchOperator);
    }
}
