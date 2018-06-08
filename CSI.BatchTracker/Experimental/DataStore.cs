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
        public ObservableCollection<BatchOperator> BatchOperators { get; set; }
        public ObservableCollection<string> Colors { get; set; }
        public ObservableCollection<ReceivedBatch> ReceivedBatches { get; set; }
        public ObservableCollection<InventoryBatch> InventoryBatches { get; set; }

        public DataStore()
        {

        }

        public void CalculateInventory()
        {
            if (InventoryBatches == null)
            {
                InventoryBatches = new ObservableCollection<InventoryBatch>();
            }

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
        }
    }
}
