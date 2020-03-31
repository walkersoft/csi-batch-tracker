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

        void UpdateImplementationLedger();
        void AddBatchToImplementationLedger(string batchNumber, DateTime date, BatchOperator batchOperator);
        void UndoImplementedBatch(int targetId);
        ObservableCollection<LoggedBatch> GetImplementedBatchesByBatchNumber(string batchNumber);
        ObservableCollection<LoggedBatch> GetConnectedBatchesAtDate(DateTime date);
    }
}
