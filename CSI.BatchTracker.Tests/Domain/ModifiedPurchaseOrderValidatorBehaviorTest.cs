using CSI.BatchTracker.Domain;
using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Exceptions;
using CSI.BatchTracker.Tests.TestHelpers.NativeModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.Domain
{
    [TestFixture]
    abstract class ModifiedPurchaseOrderValidatorBehaviorTest
    {
        protected IBatchOperatorSource operatorSource;
        protected IReceivedBatchSource receivedBatchSource;
        protected IActiveInventorySource inventorySource;
        protected IImplementedBatchSource implementedBatchSource;

        ModifiedPurchaseOrderValidator modified;
        BatchOperatorTestHelper operatorHelper;
        int originalPONumber;
        int originalBatchOperatorId;
        DateTime activityDate;
        string whiteBatch;
        string blackBatch;
        string yellowBatch;

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
            ImportReceivingRecordsIntoValidator();
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

        void ImportReceivingRecordsIntoValidator()
        {
            modified = new ModifiedPurchaseOrderValidator(
                receivedBatchSource.GetReceivedBatchesByPONumber(11111),
                inventorySource,
                implementedBatchSource
            );
        }

        [Test]
        public void ImportedReceivedRecordsIsCorrect()
        {
            Assert.AreEqual(originalPONumber, modified.PONumber);
            Assert.AreEqual(activityDate, modified.ReceivedDate);
            Assert.AreEqual("Jane Doe", modified.ReceivingOperator.FullName);
            Assert.AreEqual(3, modified.ReceivedBatches.Count);
            Assert.AreEqual(whiteBatch, modified.ReceivedBatches[0].BatchNumber);
            Assert.AreEqual(blackBatch, modified.ReceivedBatches[1].BatchNumber);
            Assert.AreEqual(yellowBatch, modified.ReceivedBatches[2].BatchNumber);
        }

        [Test]
        public void ExceptionIsThrownIfBatchRecordSetIsEmpty()
        {
            Assert.Throws<ModifiedPurchaseOrderValidationException>(
                () => new ModifiedPurchaseOrderValidator(new ObservableCollection<ReceivedBatch>(), inventorySource, implementedBatchSource)
            );
        }

        [Test]
        public void ReceivedBatchCanBeDeletedIfItHasNotBeenImplemented()
        {
            int expectedRemainingCount = 2;
            int expectedDeleteCount = 1;

            modified.ReceivedBatches.RemoveAt(1);
            modified.RunPurchaseOrderComparison();

            Assert.AreEqual(expectedRemainingCount, modified.ReceivedBatches.Count);
            Assert.AreEqual(expectedDeleteCount, modified.RecordsToDelete.Count);
        }

        [Test]
        public void ReceivedBatchCannotBeDeletedIfItHasBeenImplemented()
        {
            int expectedRemainingCount = 3;
            int expectedDeleteCount = 0;

            implementedBatchSource.AddBatchToImplementationLedger(blackBatch, DateTime.Now, operatorSource.FindBatchOperator(originalBatchOperatorId));
            modified.RunPurchaseOrderComparison();

            Assert.AreEqual(expectedRemainingCount, modified.ReceivedBatches.Count);
            Assert.AreEqual(expectedDeleteCount, modified.RecordsToDelete.Count);
        }

        [Test]
        public void ReceivedBatchesCanHavePONumberUpdated()
        {
            int expectedPONumber = 22222;
            int expectedUpdateCount = 3;

            modified.PONumber = expectedPONumber;
            modified.RunPurchaseOrderComparison();

            Assert.AreEqual(expectedPONumber, modified.RecordsToUpdate[0].PONumber);
            Assert.AreEqual(expectedPONumber, modified.RecordsToUpdate[1].PONumber);
            Assert.AreEqual(expectedPONumber, modified.RecordsToUpdate[2].PONumber);
            Assert.AreEqual(expectedUpdateCount, modified.RecordsToUpdate.Count);
        }
    }
}
