using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Storage.MemoryStore.Transactions.RecordAquisition
{
    public sealed class FindBatchesInReceivingLedgerByDateRangeTransaction : MemoryDataSourceTransaction
    {
        MemoryStoreContext memoryStore;
        DateTime start, end;

        public FindBatchesInReceivingLedgerByDateRangeTransaction(DateTime start, DateTime end, MemoryStoreContext memoryStore)
        {
            this.start = start;
            this.end = end;
            this.memoryStore = memoryStore;
        }

        public override void Execute()
        {
            Results.Clear();

            foreach (KeyValuePair<int, Entity<ReceivedBatch>> entity in memoryStore.ReceivingLedger)
            {
                if (ActivityDateIsWithinRange(entity.Value.NativeModel.ActivityDate))
                {
                    Results.Add(entity.Value);
                }
            }
        }

        bool ActivityDateIsWithinRange(DateTime activityDate)
        {
            return activityDate.Date >= start.Date
                && activityDate.Date <= end.Date;
        }
    }
}
