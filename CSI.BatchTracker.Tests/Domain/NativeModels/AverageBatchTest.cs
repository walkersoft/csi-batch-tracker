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
    class AverageBatchTest
    {
        [Test]
        public void SettingUpAverageBatchAndGettingAnAverage()
        {
            string expectedAverage = "5.00";
            int productionDays = 20;
            int quantityUsed = 100;
            AverageBatch batch = new AverageBatch("White", productionDays, quantityUsed);

            Assert.AreEqual(expectedAverage, batch.DisplayUsage);
        }

        [Test]
        public void GettingAverageWithPrecisionThatRoundsSecondDecimalUp()
        {
            string expectedAverage = "0.06";
            int productionDays = 18;
            int quantityUsed = 1;
            AverageBatch batch = new AverageBatch("White", productionDays, quantityUsed);

            Assert.AreEqual(expectedAverage, batch.DisplayUsage);
        }

        [Test]
        public void GettingAverageWithPrecisionThatRoundsSecondDecimalDown()
        {
            string expectedAverage = "0.05";
            int productionDays = 19;
            int quantityUsed = 1;
            AverageBatch batch = new AverageBatch("White", productionDays, quantityUsed);

            Assert.AreEqual(expectedAverage, batch.DisplayUsage);
        }
    }
}
