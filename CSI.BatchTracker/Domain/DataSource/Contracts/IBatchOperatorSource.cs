using CSI.BatchTracker.Domain.NativeModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CSI.BatchTracker.Domain.DataSource.Contracts
{
    public interface IBatchOperatorSource
    {
        ObservableCollection<BatchOperator> OperatorRepository { get; }
        Dictionary<int, int> BatchOperatorIdMappings { get; }

        void SaveOperator(BatchOperator batchOperator);
        void UpdateOperator(int id, BatchOperator batchOperator);
        void DeleteBatchOperator(int id);
        BatchOperator FindBatchOperator(int id);
    }
}
