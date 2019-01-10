using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.TestHelpers.NativeModels
{
    internal class ReceivedBatchTestHelper
    {
        BatchOperatorTestHelper helper;

        public ReceivedBatchTestHelper()
        {
            helper = new BatchOperatorTestHelper();
        }

        public ReceivedBatch GetUniqueBatch1()
        {
            /* Color: White
             * Batch: 872890101101
             * Date: Now
             * Qty: 5
             * PO: 55555
             * Operator: Jane Doe
             */
            return new ReceivedBatch("White", "872890101101", DateTime.Now, 5, 55555, helper.GetJaneDoeOperator());
        }

        public ReceivedBatch GetUniqueBatch2()
        {
            /* Color: Black
             * Batch: 872890101103
             * Date: Now
             * Qty: 5
             * PO: 55555
             * Operator: Jane Doe
             */
            return new ReceivedBatch("Black", "872890101103", DateTime.Now, 5, 55555, helper.GetJaneDoeOperator());
        }
    }
}
