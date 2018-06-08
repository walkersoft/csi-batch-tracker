using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Exceptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.Domain.NativeModels
{
    [TestFixture]
    public class ReceivedBatchTest
    {
        readonly string colorName = "Yellow";
        readonly string batchNumber = "872880703201";
        readonly DateTime date = DateTime.Now;
        readonly int qtyAvailable = 5;
        readonly int poNumber = 40902;
        readonly BatchOperator receivingOperator = new BatchOperator("Jane", "Doe");
        ReceivedBatch batch;

        [SetUp]
        public void SetUp()
        {
            batch = new ReceivedBatch(colorName, batchNumber, date, qtyAvailable, poNumber, receivingOperator);
        }

        [Test]
        public void ReceivedBatchIsSetupCorrectly()
        {
            Assert.AreEqual(colorName, batch.ColorName);
            Assert.AreEqual(batchNumber, batch.BatchNumber);
            Assert.AreEqual(date, batch.ActivityDate);
            Assert.AreEqual(qtyAvailable, batch.Quantity);
            Assert.AreEqual(poNumber, batch.PONumber);
            Assert.AreSame(receivingOperator, batch.ReceivingOperator);
        }

        [Test]
        public void ExceptionIfColorNameIsEmpty()
        {
            Assert.Throws<BatchException>(() => new ReceivedBatch("", batchNumber, date, qtyAvailable, poNumber, receivingOperator));
        }

        [Test]
        public void ExceptionIfBatchNumberIsEmpty()
        {
            Assert.Throws<BatchException>(() => new ReceivedBatch(colorName, "", date, qtyAvailable, poNumber, receivingOperator));
        }

        [Test]
        public void ExceptionIfQuantityIsLessThanOne()
        {
            Assert.Throws<BatchException>(() => new ReceivedBatch(colorName, batchNumber, date, 0, poNumber, receivingOperator));
        }

        [Test]
        public void ExceptionIfPONumberIsNotAtLeastFiveDigits()
        {
            Assert.Throws<BatchException>(() => new ReceivedBatch(colorName, batchNumber, date, qtyAvailable, 1111, receivingOperator));
        }

        [Test]
        public void PoNumberCanBeBiggerThanFiveDigits()
        {
            batch = new ReceivedBatch(colorName, batchNumber, date, qtyAvailable, 111111, receivingOperator);
        }
    }
}
