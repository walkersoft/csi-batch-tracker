using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Exceptions
{
    public sealed class ModifiedPurchaseOrderValidationException : Exception
    {
        public ModifiedPurchaseOrderValidationException(string message) : base(message)
        {
        }
    }
}
