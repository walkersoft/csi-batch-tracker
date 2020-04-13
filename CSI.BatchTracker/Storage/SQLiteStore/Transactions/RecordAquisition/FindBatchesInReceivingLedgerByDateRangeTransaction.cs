using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;

namespace CSI.BatchTracker.Storage.SQLiteStore.Transactions.RecordAquisition
{
    public sealed class FindBatchesInReceivingLedgerByDateRangeTransaction : SQLiteDataSourceTransaction
    {
        DateTime start;
        DateTime end;
        SQLiteStoreContext store;

        public FindBatchesInReceivingLedgerByDateRangeTransaction(DateTime start, DateTime end, SQLiteStoreContext store)
        {
            this.store = store;
            this.start = start;
            this.end = end;
        }

        public override void Execute()
        {
            string query = "SELECT * FROM ReceivedBatches WHERE datetime(ReceivingDate) >= datetime(?) AND datetime(ReceivingDate) <= datetime(?) ORDER BY ReceivingDate DESC";

            List<object> parameters = new List<object>
            {
                start.FormatForDatabase(),
                end.FormatForDatabase()
            };

            store.ExecuteReader(typeof(ReceivedBatch), query, parameters);
            Results = store.Results;
        }
    }
}
