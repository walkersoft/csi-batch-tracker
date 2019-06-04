using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Tests.TestHelpers.NativeModels;
using NUnit.Framework;
using System;
using System.Collections.ObjectModel;

namespace CSI.BatchTracker.Tests.Domain.NativeModels
{
    [TestFixture]
    class ReceivedPurchaseOrderTest
    {
        ReceivedBatchTestHelper helper;

        [SetUp]
        public void SetUp()
        {
            helper = new ReceivedBatchTestHelper();
        }

        [Test]
        public void CreateReceivedPurchaseOrderFromExistingData()
        {
            ReceivedBatch received = helper.GetBatchWithSpecificDate(new DateTime(2019, 7, 4));
            int expectedPONumber = received.PONumber;
            DateTime expectedActivityDate = received.ActivityDate;
            BatchOperator expectedBatchOperator = received.ReceivingOperator;
            int expectedCount = 3;
            string expectedDateDisplay = "July 4, 2019";

            ObservableCollection<ReceivedBatch> receivedBatches = new ObservableCollection<ReceivedBatch>()
            {
                received, received, received
            };

            ReceivedPurchaseOrder receivedPO = new ReceivedPurchaseOrder(
                expectedPONumber, 
                expectedActivityDate, 
                expectedBatchOperator,
                receivedBatches
            );

            Assert.AreEqual(expectedPONumber, receivedPO.PONumber);
            Assert.AreEqual(expectedActivityDate, receivedPO.ActivityDate);
            Assert.AreEqual(expectedDateDisplay, receivedPO.DisplayDate);
            Assert.AreSame(expectedBatchOperator, receivedPO.ReceivingOperator);
            Assert.AreEqual(expectedCount, receivedPO.ReceivedBatches.Count);
        }

        [Test]
        public void CreatedReceivedPurchaseOrderWithAnEmptySetOfReceivedBatches()
        {
            int expectedCount = 0;
            ReceivedBatch received = helper.GetUniqueBatch1();

            ReceivedPurchaseOrder receivedPO = new ReceivedPurchaseOrder(
                received.PONumber,
                received.ActivityDate,
                received.ReceivingOperator
            );

            Assert.AreEqual(expectedCount, receivedPO.ReceivedBatches.Count);
        }

        [Test]
        public void CreatedReceivedPurchaseOrderAndAddReceivedBatchesToIt()
        {
            int expectedCount = 1;
            ReceivedBatch received = helper.GetUniqueBatch1();

            ReceivedPurchaseOrder receivedPO = new ReceivedPurchaseOrder(
                received.PONumber,
                received.ActivityDate,
                received.ReceivingOperator
            );

            receivedPO.AddBatch(received);

            Assert.AreEqual(expectedCount, receivedPO.ReceivedBatches.Count);
        }
    }
}
