using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            string query = "SELECT * FROM ReceivedBatches WHERE datetime(ReceivingDate) >= datetime(?) AND datetime(ReceivingDate) < datetime(?)";

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
