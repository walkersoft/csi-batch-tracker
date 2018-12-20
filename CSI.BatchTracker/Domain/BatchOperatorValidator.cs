using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Domain
{
    public sealed class BatchOperatorValidator
    {
        public BatchOperatorValidator() { }

        public bool Validate(BatchOperator batchOperator)
        {
            return NameIsCorrect(batchOperator.FirstName) && NameIsCorrect(batchOperator.LastName);
        }

        bool NameIsCorrect(string name)
        {
            return string.IsNullOrWhiteSpace(name) == false;
        }
    }
}
