using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Contracts
{
    abstract public class AbstractBatch
    {
        public string ColorName { get; protected set; }
        public string BatchNumber { get; protected set; }
        public DateTime ActivityDate { get; protected set; }
    }
}
