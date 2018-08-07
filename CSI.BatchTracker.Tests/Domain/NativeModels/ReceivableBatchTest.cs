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
    class ReceivableBatchTest
    {
        readonly string colorName = "Yellow";
        readonly string batchNumber = "872880703201";
        readonly DateTime date = DateTime.Now;
        readonly int quantity = 5;
        ReceivableBatch batch;

        [Test]
        public void ReceivableBatchIsSetup()
        {
            batch = new ReceivableBatch(colorName, batchNumber, quantity);

            Assert.AreEqual(colorName, batch.ColorName);
            Assert.AreEqual(batchNumber, batch.BatchNumber);
            Assert.AreEqual(quantity, batch.Quantity);
        }

        [Test]
        public void ExceptionIfColorNameIsEmpty()
        {
            Assert.Throws<BatchException>(() => new ReceivableBatch("", batchNumber, quantity));
        }

        [Test]
        public void ExceptionIfBatchNumberIsEmpty()
        {
            Assert.Throws<BatchException>(() => new ReceivableBatch(colorName, "", quantity));
        }

        [Test]
        public void ExceptionIfQuantityIsLessThanOne()
        {
            Assert.Throws<BatchException>(() => new ReceivableBatch(colorName, batchNumber, 0));
        }
    }
}
