using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Domain.DataSource.Contracts
{
    public interface IBatchOperatorSource
    {
        ObservableCollection<BatchOperator> OperatorRepository { get; }
        Dictionary<int, int> BatchOperatorIdMappings { get; }

        void SaveOperator(BatchOperator batchOperator);
        void UpdateOperator(int id, BatchOperator batchOperator);
        void DeleteBatchOperatorAtId(int id);
        BatchOperator FindBatchOperatorById(int id);
    }
}
