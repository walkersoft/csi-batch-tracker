using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;

namespace CSI.BatchTracker.Storage.SQLiteStore.Transactions.RecordAquisition
{
    public sealed class ListImplementedBatchTransaction : SQLiteDataSourceTransaction
    {
        SQLiteStoreContext store;

        public ListImplementedBatchTransaction(SQLiteStoreContext store)
        {
            this.store = store;
        }

        public override void Execute()
        {
            DateTime latest = GetLatestImplementationDate();
            string query = "SELECT * FROM ImplementedBatches WHERE datetime(ImplementationDate) >= ? AND datetime(ImplementationDate) <= ? ORDER BY ImplementationDate DESC";

            List<object> parameters = new List<object>()
            {
                latest.AddDays(-30).FormatForDatabase(),
                latest.FormatForDatabase()
            };

            store.ExecuteReader(typeof(LoggedBatch), query, parameters);
            Results = store.Results;
        }

        DateTime GetLatestImplementationDate()
        {
            string query = "SELECT * FROM ImplementedBatches WHERE datetime(ImplementationDate) <= datetime(?) ORDER BY ImplementationDate DESC LIMIT 1";
            List<object> parameters = new List<object>() { DateTime.Now.FormatForDatabase() };
            store.ExecuteReader(typeof(LoggedBatch), query, parameters);
            Results = store.Results;

            if (Results.Count > 0)
            {
                Entity<LoggedBatch> entity = Results[0] as Entity<LoggedBatch>;
                return entity.NativeModel.ActivityDate;
            }

            return DateTime.Now;
        }
    }
}
