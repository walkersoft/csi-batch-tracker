using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;

namespace CSI.BatchTracker.Storage.SQLiteStore.Transactions.RecordAquisition
{
    public sealed class FindBatchesInReceivingLedgerByDateTransaction : SQLiteDataSourceTransaction
    {
        SQLiteStoreContext store;
        DateTime date;

        public FindBatchesInReceivingLedgerByDateTransaction(DateTime date, SQLiteStoreContext store)
        {
            this.store = store;
            this.date = date;
        }

        public override void Execute()
        {
            DateTime start = new DateTime(date.Year, date.Month, date.Day);
            DateTime end = start.AddDays(1).AddSeconds(-1);
            string query = "SELECT * FROM ReceivedBatches WHERE datetime(ReceivingDate) >= datetime(?) AND datetime(ReceivingDate) < datetime(?) ORDER BY ReceivingDate DESC";

            List<object> parameters = new List<object>()
            {
                start.FormatForDatabase(),
                end.FormatForDatabase()
            };

            store.ExecuteReader(typeof(ReceivedBatch), query, parameters);
            Results = store.Results;
        }
    }
}
