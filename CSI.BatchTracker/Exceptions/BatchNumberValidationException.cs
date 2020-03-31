using System;

namespace CSI.BatchTracker.Exceptions
{
    public sealed class BatchNumberValidationException : Exception
    {
        public BatchNumberValidationException(string message) : base(message) { }
    }
}
