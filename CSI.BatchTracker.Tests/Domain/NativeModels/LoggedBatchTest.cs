using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Exceptions;
using NUnit.Framework;
using System;

namespace CSI.BatchTracker.Tests.Domain.NativeModels
{
    [TestFixture]
    class LoggedBatchTest
    {
        readonly string colorName = "Deep Green";
        readonly string batchNumber = "872280503401";
        readonly DateTime implementationDate = DateTime.Now;
        readonly BatchOperator implementingOperator = new BatchOperator("Jane", "Doe");

        LoggedBatch batch;

        [SetUp]
        public void SetUp()
        {
            batch = new LoggedBatch(colorName, batchNumber, implementationDate, implementingOperator);
        }

        [Test]
        public void LoggedBatchIsSetupCorrectly()
        {

            Assert.AreEqual(colorName, batch.ColorName);
            Assert.AreEqual(batchNumber, batch.BatchNumber);
            Assert.AreEqual(implementationDate, batch.ActivityDate);
            Assert.AreEqual(implementingOperator, batch.ImplementingOperator);
        }

        [Test]
        public void ExceptionIfColorNameIsEmpty()
        {
            Assert.Throws<BatchException>(() => new LoggedBatch("", batchNumber, implementationDate, implementingOperator));
        }

        [Test]
        public void ExceptionIfBatchNumberIsEmpty()
        {
            Assert.Throws<BatchException>(() => new LoggedBatch(colorName, "", implementationDate, implementingOperator));
        }

        [Test]
        public void DisplayDateIsFormattedCorrectly()
        {
            string expectedDisplayDate = "January 1, 2020 5:23 AM";
            DateTime inputDate = new DateTime(2020, 1, 1, 5, 23, 59);
            batch.ActivityDate = inputDate;

            Assert.AreEqual(expectedDisplayDate, batch.DisplayDate);
        }
    }
}
