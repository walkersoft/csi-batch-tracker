using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Exceptions
{
    public class BatchException : Exception
    {
        public BatchException(string message) : base(message)
        {

        }
    }
}
