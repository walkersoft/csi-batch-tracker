using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CSI.BatchTracker.Domain.DataSource.Contracts
{
    public interface IReceivedBatchSource
    {
        ObservableCollection<ReceivedBatch> ReceivedBatchRepository { get; }
        Dictionary<int, int> ReceivedBatchIdMappings { get; }

        void SaveReceivedBatch(ReceivedBatch receivedBatch);
        void UpdateReceivedBatch(int id, ReceivedBatch batch);
        void DeleteReceivedBatch(int id);
        ReceivedBatch FindReceivedBatchById(int id);
        ObservableCollection<ReceivedBatch> GetReceivedBatchesByBatchNumber(string batchNumber);
        ObservableCollection<ReceivedBatch> GetReceivedBatchesWithinDateRange(DateTime startDate, DateTime endDate);
        ObservableCollection<ReceivedBatch> GetReceivedBatchesByPONumber(int poNumber);
        void FindReceivedBatchesByPONumber(int poNumber);
        void FindReceivedBatchesByDate(DateTime date);
        void FindAllReceivedBatches();
    }
}
