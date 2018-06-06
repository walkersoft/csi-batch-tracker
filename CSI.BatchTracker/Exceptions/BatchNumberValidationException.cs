using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Exceptions
{
    public class BatchNumberValidationException : Exception
    {
        public BatchNumberValidationException(string message) : base(message)
        {
        }
    }
}
