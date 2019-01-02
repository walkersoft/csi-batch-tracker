using System;

namespace CSI.BatchTracker.Exceptions
{
    public class BatchNumberValidationException : Exception
    {
        public BatchNumberValidationException(string message) : base(message)
        {
        }
    }
}
