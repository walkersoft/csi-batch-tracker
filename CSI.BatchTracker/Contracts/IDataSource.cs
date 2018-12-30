using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Contracts
{
    public interface IDataSource
    {
        ObservableCollection<InventoryBatch> InventoryRepository { get; }
        ObservableCollection<BatchOperator> OperatorRepository { get; }
        ObservableCollection<LoggedBatch> BatchLedger { get; }

        Dictionary<int, int> BatchOperatorIdMappings { get; }

        void ReceiveInventory(ReceivedBatch batch);
        void SaveOperator(BatchOperator batchOperator);
        void UpdateOperator(int id, BatchOperator batchOperator);
        void DeleteBatchOperatorAtId(int id);
        BatchOperator FindBatchOperatorById(int id);
        void ImplementBatch(string batchNumber, DateTime implementationDate, BatchOperator batchOperator);
    }
}
