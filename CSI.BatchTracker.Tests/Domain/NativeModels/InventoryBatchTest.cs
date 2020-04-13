using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Exceptions;
using NUnit.Framework;
using System;

namespace CSI.BatchTracker.Tests.Domain.NativeModels
{
    [TestFixture]
    class InventoryBatchTest
    {
        readonly string colorName = "Bright Red";
        readonly string batchNumber = "872880403204";
        readonly DateTime inventoryDate = DateTime.Now;
        readonly int quantity = 4;

        InventoryBatch batch;

        [SetUp]
        public void Setup()
        {
            batch = new InventoryBatch(colorName, batchNumber, inventoryDate, quantity);
        }

        [Test]
        public void InventoryBatchIsSetupCorrectly()
        {
            Assert.AreEqual(colorName, batch.ColorName);
            Assert.AreEqual(batchNumber, batch.BatchNumber);
            Assert.AreEqual(inventoryDate, batch.ActivityDate);
            Assert.AreEqual(quantity, batch.Quantity);
        }

        [Test]
        public void ViewDisplayName()
        {
            string expected = "Bright Red - 872880403204";
            Assert.AreEqual(expected, batch.DisplayName);
        }

        [Test]
        public void DeductFromQuantity()
        {
            int expected = 3;
            batch.DeductQuantity(1);

            Assert.AreEqual(expected, batch.Quantity);
        }

        [Test]
        public void AddToQuantity()
        {
            int expected = 5;
            batch.AddQuantity(1);

            Assert.AreEqual(expected, batch.Quantity);
        }

        [Test]
        public void ExceptionIfTryingToAddInventoryWithQuantityLessThanOne()
        {
            Assert.Throws<BatchException>(() => batch.AddQuantity(0));
        }

        [Test]
        public void ExceptionIfTryingToDeductInventoryWithQuantityLessThanOne()
        {
            Assert.Throws<BatchException>(() => batch.DeductQuantity(0));
        }

        [Test]
        public void ExceptionInstantiatingWithQuantityLessThanOne()
        {
            Assert.Throws<BatchException>(() => new InventoryBatch(colorName, batchNumber, inventoryDate, 0));
        }

        [Test]
        public void ExceptionIfColorNameIsEmpty()
        {
            Assert.Throws<BatchException>(() => new InventoryBatch("", batchNumber, inventoryDate, quantity));
        }

        [Test]
        public void ExceptionIfBatchNumberIsEmpty()
        {
            Assert.Throws<BatchException>(() => new InventoryBatch(colorName, "", inventoryDate, quantity));
        }

        [Test]
        public void DisplayDateIsFormattedCorrectly()
        {
            string expectedDisplayDate = "January 1, 2020";
            DateTime inputDate = new DateTime(2020, 1, 1, 5, 23, 59);
            batch.ActivityDate = inputDate;

            Assert.AreEqual(expectedDisplayDate, batch.DisplayDate);
        }
    }
}
