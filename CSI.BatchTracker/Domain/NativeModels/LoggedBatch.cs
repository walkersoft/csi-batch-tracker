using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Domain.NativeModels
{
    public class LoggedBatch
    {
        public string ColorName { get; private set; }
        public string BatchNumber { get; private set; }
        public DateTime ImplementationDate { get; private set; }
        public BatchOperator ImplementingOperator { get; private set; }

        public LoggedBatch(string colorName, string batchNumber, DateTime implementationDate, BatchOperator implementationOperator)
        {
            ColorName = colorName;
            BatchNumber = batchNumber;
            ImplementationDate = implementationDate;
            ImplementingOperator = implementationOperator;
        }
    }
}
