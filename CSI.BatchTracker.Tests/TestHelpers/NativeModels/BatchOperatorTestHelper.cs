using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.TestHelpers.NativeModels
{
    public class BatchOperatorTestHelper
    {
        public BatchOperator GetJaneDoeOperator()
        {
            return new BatchOperator("Jane", "Doe");
        }

        public BatchOperator GetJohnDoeOperator()
        {
            return new BatchOperator("John", "Doe");
        }
    }
}
