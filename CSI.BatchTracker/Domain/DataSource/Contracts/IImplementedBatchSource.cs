using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CSI.BatchTracker.Domain.DataSource.Contracts
{
    public interface IImplementedBatchSource
    {
        ObservableCollection<LoggedBatch> ImplementedBatchLedger { get; }
        Dictionary<int, int> ImplementedBatchIdMappings { get; }
        void AddBatchToImplementationLedger(string batchNumber, DateTime date, BatchOperator batchOperator);
        ObservableCollection<LoggedBatch> GetImplementedBatchesByBatchNumber(string batchNumber);
        void UndoImplementedBatch(int targetId);
    }
}
