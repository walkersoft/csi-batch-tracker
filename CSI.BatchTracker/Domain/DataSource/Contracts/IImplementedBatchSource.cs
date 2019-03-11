using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.ObjectModel;

namespace CSI.BatchTracker.Domain.DataSource.Contracts
{
    public interface IImplementedBatchSource
    {
        ObservableCollection<LoggedBatch> ImplementedBatchLedger { get; }
        void AddBatchToImplementationLedger(string batchNumber, DateTime date, BatchOperator batchOperator);
        void UndoImplementedBatch(int targetId);
    }
}
