using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CSI.BatchTracker.Experimental
{
    public class DataStore
    {
        public Dictionary<int, Entity<BatchOperator>> BatchOperators { get; set; }
        public ObservableCollection<string> Colors { get; set; }
        public ObservableCollection<ReceivedBatch> ReceivedBatches { get; set; }
        public Dictionary<int, Entity<InventoryBatch>> InventoryBatches { get; set; }
        public ObservableCollection<LoggedBatch> LoggedBatches { get; set; }

        public DataStore()
        {
            LoggedBatches = new ObservableCollection<LoggedBatch>();
            InventoryBatches = new Dictionary<int, Entity<InventoryBatch>>();
            ReceivedBatches = new ObservableCollection<ReceivedBatch>();
            BatchOperators = new Dictionary<int, Entity<BatchOperator>>();
        }

        public void CalculateInventory()
        {
            foreach (ReceivedBatch batch in ReceivedBatches)
            {
                bool found = false;

                for (int i = 1; i <= InventoryBatches.Count; ++i)
                {
                    InventoryBatch stockBatch;

                    if (InventoryBatches.ContainsKey(i))
                    {
                        stockBatch = InventoryBatches[i].NativeModel;

                        if (batch.BatchNumber == stockBatch.BatchNumber)
                        {
                            found = true;
                            stockBatch.AddQuantity(batch.Quantity);
                        }
                    }
                }                    

                if (!found)
                {
                    InventoryBatch newStockBatch = new InventoryBatch(batch.ColorName, batch.BatchNumber, batch.ActivityDate, batch.Quantity);
                    int nextId = InventoryBatches.Count + 1; //this is not accurate, but for prototyping will be okay.
                    InventoryBatches.Add(nextId, new Entity<InventoryBatch>(nextId, newStockBatch));
                }
            }

            ReceivedBatches.Clear();
        }

        public void ImplementBatch(string batchNumber, DateTime activityDate, BatchOperator implentingOperator)
        {
            for (int i = 1; i <= InventoryBatches.Count; ++i)
            {
                InventoryBatch stockBatch;

                if (InventoryBatches.ContainsKey(i))
                {
                    stockBatch = InventoryBatches[i].NativeModel;

                    if (stockBatch.BatchNumber == batchNumber)
                    {
                        LoggedBatch loggedBatch = new LoggedBatch(
                            stockBatch.ColorName,
                            stockBatch.BatchNumber,
                            activityDate,
                            implentingOperator
                        );

                        LoggedBatches.Add(loggedBatch);
                        stockBatch.DeductQuantity(1);
                    }
                }
            }
        }
    }
}
