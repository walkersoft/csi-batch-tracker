using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using System;

namespace CSI.BatchTracker.Tests.TestHelpers.NativeModels
{
    internal class ReceivedBatchTestHelper
    {
        BatchOperatorTestHelper helper;
        IBatchOperatorSource operatorSource;

        public ReceivedBatchTestHelper(IBatchOperatorSource operatorSource)
        {
            this.operatorSource = operatorSource;
            helper = new BatchOperatorTestHelper(this.operatorSource);
            operatorSource.SaveOperator(helper.GetUnsavedJaneDoeOperator());
        }

        public ReceivedBatch GetUniqueBatch1()
        {
            return new ReceivedBatch("White", "872890101101", DateTime.Now, 5, 55555, operatorSource.FindBatchOperator(1));
        }

        public ReceivedBatch GetUniqueBatch2()
        {
            return new ReceivedBatch("Black", "872890101103", DateTime.Now, 5, 55555, operatorSource.FindBatchOperator(1));
        }

        public ReceivedBatch GetBatchWithSpecificPO(int poNumber)
        {
            ReceivedBatch batch = GetUniqueBatch1();
            batch.PONumber = poNumber;
            return batch;
        }

        public ReceivedBatch GetBatchWithSpecificDate(DateTime dateCriteria)
        {
            ReceivedBatch batch = GetUniqueBatch1();
            batch.ActivityDate = dateCriteria;
            return batch;
        }
    }
}
