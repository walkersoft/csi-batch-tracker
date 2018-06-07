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
    public class LoggedBatchTest
    {
        [Test]
        public void LoggedBatchIsSetupCorrectly()
        {
            string colorName = "Deep Green";
            string batchNumber = "872280503401";
            DateTime implementationDate = DateTime.Now;
            BatchOperator implementingOperator = new BatchOperator("Jane", "Doe");

            LoggedBatch batch = new LoggedBatch(colorName, batchNumber, implementationDate, implementingOperator);

            Assert.AreEqual(colorName, batch.ColorName);
            Assert.AreEqual(batchNumber, batch.BatchNumber);
            Assert.AreEqual(implementationDate, batch.ImplementationDate);
            Assert.AreEqual(implementingOperator, batch.ImplementingOperator);
        }
    }
}
