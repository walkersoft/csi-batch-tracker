using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Storage.Contracts;
using System;
using System.Collections.Generic;

namespace CSI.BatchTracker.Storage.SQLiteStore.Transactions.RecordAquisition
{
    public sealed class ListConnectedBatchesAtDateTransaction : SQLiteDataSourceTransaction
    {
        SQLiteStoreContext store;
        DateTime calibratedDate;

        public ListConnectedBatchesAtDateTransaction(DateTime targetDate, SQLiteStoreContext store)
        {
            this.store = store;
            calibratedDate = new DateTime(targetDate.Year, targetDate.Month, targetDate.Day, 23, 59, 59);
        }

        public override void Execute()
        {
            List<string> colors = new List<string> { "White", "Black", "Yellow", "Red", "Blue Red", "Deep Green", "Deep Blue", "Bright Red", "Bright Yellow" };
            string query = "SELECT * FROM ImplementedBatches WHERE ColorName = ? AND datetime(ImplementationDate) <= datetime(?) ORDER BY ImplementationDate DESC LIMIT 3";
            
            foreach (string color in colors)
            {
                List<object> parameters = new List<object>() { color, calibratedDate.FormatForDatabase() };
                store.ExecuteReader(typeof(LoggedBatch), query, parameters);
                FilterUniqueBatches(store.Results);
            }
        }

        void FilterUniqueBatches(List<IEntity> results)
        {            
            string lastBatchNumber = string.Empty;
            DateTime lastDate = calibratedDate;

            for (int i = 0; i < results.Count; i++)
            {
                Entity<LoggedBatch> entity = results[i] as Entity<LoggedBatch>;

                if (lastBatchNumber == string.Empty)
                {
                    Results.Add(results[i]);
                    lastBatchNumber = entity.NativeModel.BatchNumber;
                    lastDate = entity.NativeModel.ActivityDate;
                    continue;
                }

                if (DatesAreOnSameDayAsQueryDate(entity.NativeModel.ActivityDate, lastDate))
                {
                    if (lastBatchNumber != entity.NativeModel.BatchNumber)
                    {
                        Results.Add(results[i]);
                        continue;
                    }
                }
            }
        }

        bool DatesAreOnSameDayAsQueryDate(DateTime first, DateTime second)
        {
            return first.Year == second.Year && first.DayOfYear == second.DayOfYear;
        }
    }
}
