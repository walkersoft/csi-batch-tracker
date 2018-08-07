using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Experimental
{
    public class DataStore
    {
        public Dictionary<int, Entity<BatchOperator>> BatchOperators { get; set; }
        public ObservableCollection<string> Colors { get; set; }
        public ObservableCollection<ReceivedBatch> ReceivedBatches { get; set; }
        public ObservableCollection<InventoryBatch> InventoryBatches { get; set; }
        public ObservableCollection<LoggedBatch> LoggedBatches { get; set; }

        public DataStore()
        {
            LoggedBatches = new ObservableCollection<LoggedBatch>();
            InventoryBatches = new ObservableCollection<InventoryBatch>();
            ReceivedBatches = new ObservableCollection<ReceivedBatch>();
            BatchOperators = new Dictionary<int, Entity<BatchOperator>>();
        }

        public void CalculateInventory()
        {
            foreach (ReceivedBatch batch in ReceivedBatches)
            {
                bool found = false;

                foreach (InventoryBatch stockBatch in InventoryBatches)
                {
                    if (batch.BatchNumber == stockBatch.BatchNumber)
                    {
                        found = true;
                        stockBatch.AddQuantity(batch.Quantity);
                    }
                }

                if (!found)
                {
                    InventoryBatch newStockBatch = new InventoryBatch(batch.ColorName, batch.BatchNumber, batch.ActivityDate, batch.Quantity);
                    InventoryBatches.Add(newStockBatch);
                }
            }

            ReceivedBatches.Clear();
        }

        public void ImplementBatch(string batchNumber, DateTime activityDate, BatchOperator implentingOperator)
        {
            foreach (InventoryBatch stockBatch in InventoryBatches)
            {
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
