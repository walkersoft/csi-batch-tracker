using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Contracts
{
    abstract public class AbstractBatchNumberValidator : IBatchNumberValidator
    {
        public int BatchNumberLength { get; protected set; }

        abstract public bool Validate(string batchNumber);

        public int GetBatchNumberLength()
        {
            return BatchNumberLength;
        }
    }
}
