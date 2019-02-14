using CSI.BatchTracker.Domain.NativeModels;

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
