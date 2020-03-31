using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;

namespace CSI.BatchTracker.Tests.TestHelpers.NativeModels
{
    internal class BatchOperatorTestHelper
    {
        IBatchOperatorSource operatorSource;

        public BatchOperatorTestHelper(IBatchOperatorSource operatorSource)
        {
            this.operatorSource = operatorSource;
        }

        public BatchOperator GetJaneDoeOperator()
        {
            operatorSource.SaveOperator(new BatchOperator("Jane", "Doe"));
            return operatorSource.FindBatchOperator(1);
        }

        public BatchOperator GetJohnDoeOperator()
        {
            operatorSource.SaveOperator(new BatchOperator("John", "Doe"));
            return operatorSource.FindBatchOperator(2);
        }

        public BatchOperator GetUnsavedJaneDoeOperator()
        {
            return new BatchOperator("Jane", "Doe");
        }

        public BatchOperator GetUnsavedJohnDoeOperator()
        {
            return new BatchOperator("John", "Doe");
        }
    }
}
