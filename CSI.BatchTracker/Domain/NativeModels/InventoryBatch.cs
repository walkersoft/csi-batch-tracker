using System;

namespace CSI.BatchTracker.Domain.NativeModels
{
    public class InventoryBatch : AbstractBatch
    {
        public int Quantity { get; private set; }

        public string DisplayName
        {
            get
            {
                return string.Format("{0} - {1}", ColorName, BatchNumber);
            }
        }

        public InventoryBatch() { }

        public InventoryBatch(string colorName, string batchNumber, DateTime inventoryDate, int quantity)
        {
            CheckIfQuantityIsGreaterThanZero(quantity);
            CheckIfColorNameIsEmpty(colorName);
            CheckIfBatchNumberIsEmpty(batchNumber);

            ColorName = colorName;
            BatchNumber = batchNumber;
            ActivityDate = inventoryDate;
            Quantity = quantity;
        }

        public void DeductQuantity(int amount)
        {
            CheckIfQuantityIsGreaterThanZero(amount);
            Quantity -= amount;
        }

        public void AddQuantity(int amount)
        {
            CheckIfQuantityIsGreaterThanZero(amount);
            Quantity += amount;
        }
    }
}
