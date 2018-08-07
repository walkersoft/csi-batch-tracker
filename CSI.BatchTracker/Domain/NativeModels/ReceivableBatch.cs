using CSI.BatchTracker.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Domain.NativeModels
{
    public class ReceivableBatch : AbstractBatch
    {
        public int Quantity { get; private set; }

        public ReceivableBatch(string colorName, string batchNumber, int quantity)
        {
            CheckIfColorNameIsEmpty(colorName);
            CheckIfBatchNumberIsEmpty(batchNumber);
            CheckIfQuantityIsGreaterThanZero(quantity);

            ColorName = colorName;
            BatchNumber = batchNumber;
            Quantity = quantity;
            ActivityDate = DateTime.Now;
        }
    }
}
