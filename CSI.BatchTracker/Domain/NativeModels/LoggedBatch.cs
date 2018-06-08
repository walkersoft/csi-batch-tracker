using CSI.BatchTracker.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Domain.NativeModels
{
    public class LoggedBatch : AbstractBatch
    {
        public BatchOperator ImplementingOperator { get; private set; }

        public LoggedBatch(string colorName, string batchNumber, DateTime implementationDate, BatchOperator implementationOperator)
        {
            ColorName = colorName;
            BatchNumber = batchNumber;
            ActivityDate = implementationDate;
            ImplementingOperator = implementationOperator;
        }
    }
}
