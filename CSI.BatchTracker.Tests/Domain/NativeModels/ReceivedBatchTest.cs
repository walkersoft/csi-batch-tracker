using CSI.BatchTracker.Domain.NativeModels;
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
        [Test]
        public void ReceivedBatchIsSetupCorrectly()
        {
            string colorName = "Yellow";
            string batchNumber = "872880703201";
            DateTime date = DateTime.Now;
            int qtyAvailable = 5;
            BatchOperator receivingOperator = new BatchOperator("Jane", "Doe");

            ReceivedBatch batch = new ReceivedBatch(colorName, batchNumber, date, qtyAvailable, receivingOperator);

            Assert.AreEqual(colorName, batch.ColorName);
            Assert.AreEqual(batchNumber, batch.BatchNumber);
            Assert.AreEqual(date, batch.ReceivingDate);
            Assert.AreEqual(qtyAvailable, batch.Quantity);
            Assert.AreSame(receivingOperator, batch.ReceivingOperator);
        }
    }
}
