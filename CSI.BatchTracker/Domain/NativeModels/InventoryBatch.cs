using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Domain.NativeModels
{
    public class InventoryBatch
    {
        public string ColorName { get; private set; }
        public string BatchNumber { get; private set; }
        public DateTime InventoryDate { get; private set; }
        public int Quantity { get; private set; }

        public InventoryBatch(string colorName, string batchNumber, DateTime inventoryDate, int quantity)
        {
            ColorName = colorName;
            BatchNumber = batchNumber;
            InventoryDate = inventoryDate;
            Quantity = quantity;
        }

        public void DeductQuantity(int amount)
        {
            Quantity -= amount;
        }

        public void AddQuantity(int amount)
        {
            Quantity += amount;
        }
    }
}
