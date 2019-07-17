using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Tests.TestHelpers.NativeModels;
using CSI.BatchTracker.ViewModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors
{
    [TestFixture]
    abstract class ReceivedPurchaseOrderEditorViewModelTestingBase
    {
        protected IBatchOperatorSource operatorSource;
        protected IReceivedBatchSource receivedBatchSource;
        protected IActiveInventorySource inventorySource;
        protected IImplementedBatchSource implementedBatchSource;
        protected ICommand command;
        protected ReceivedPurchaseOrderEditorViewModel viewModel;

        BatchOperatorTestHelper operatorHelper;
        protected int originalPONumber;
        protected int originalBatchOperatorId;
        protected DateTime activityDate;
        protected string whiteBatch;
        protected string blackBatch;
        protected string yellowBatch;

        [SetUp]
        public virtual void SetUp()
        {
            originalPONumber = 11111;
            originalBatchOperatorId = 1;
            whiteBatch = "872890501205";
            blackBatch = "872891602302";
            yellowBatch = "872891105303";
            operatorHelper = new BatchOperatorTestHelper();
            SetupPurchaseOrderState();
        }

        void SetupPurchaseOrderState()
        {
            InsertTwoUniqueBatchOperatorsIntoDataSource();
            InsertThreeUniqueBatchesIntoDataSource();
        }

        void InsertTwoUniqueBatchOperatorsIntoDataSource()
        {
            operatorSource.SaveOperator(operatorHelper.GetJaneDoeOperator());
            operatorSource.SaveOperator(operatorHelper.GetJohnDoeOperator());
        }

        void InsertThreeUniqueBatchesIntoDataSource()
        {
            BatchOperator batchOperator = operatorSource.FindBatchOperator(originalBatchOperatorId);
            activityDate = DateTime.Today;

            ReceivedBatch batch1 = new ReceivedBatch("White", whiteBatch, activityDate, 5, originalPONumber, batchOperator);
            ReceivedBatch batch2 = new ReceivedBatch("Black", blackBatch, activityDate, 5, originalPONumber, batchOperator);
            ReceivedBatch batch3 = new ReceivedBatch("Yellow", yellowBatch, activityDate, 5, originalPONumber, batchOperator);

            receivedBatchSource.SaveReceivedBatch(batch1);
            receivedBatchSource.SaveReceivedBatch(batch2);
            receivedBatchSource.SaveReceivedBatch(batch3);
        }
    }
}
