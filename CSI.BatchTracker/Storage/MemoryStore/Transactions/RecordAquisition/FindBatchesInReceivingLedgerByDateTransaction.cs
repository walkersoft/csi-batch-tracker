using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;

namespace CSI.BatchTracker.Storage.MemoryStore.Transactions.RecordAquisition
{
    public sealed class FindBatchesInReceivingLedgerByDateTransaction : MemoryDataSourceTransaction
    {
        MemoryStoreContext memoryStore;
        DateTime date;
        
        public FindBatchesInReceivingLedgerByDateTransaction(DateTime date, MemoryStoreContext memoryStore)
        {
            this.date = date;
            this.memoryStore = memoryStore;
        }

        public override void Execute()
        {
            Results.Clear();

            foreach (KeyValuePair<int, Entity<ReceivedBatch>> entity in memoryStore.ReceivingLedger)
            {
                if (ActivityDateMatchesTargetDate(entity.Value.NativeModel.ActivityDate))
                {
                    Results.Add(entity.Value);
                }
            }
        }

        bool ActivityDateMatchesTargetDate(DateTime activityDate)
        {
            return activityDate.DayOfYear == date.DayOfYear;
        }
    }
}
