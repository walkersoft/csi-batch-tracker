using System;

namespace CSI.BatchTracker.Exceptions
{
    public sealed class BatchException : Exception
    {
        public BatchException(string message) : base(message) { }
    }
}
