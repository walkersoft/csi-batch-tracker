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
    public class InventoryBatchTest
    {
        readonly string colorName = "BrightRed";
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
            Assert.AreEqual(inventoryDate, batch.InventoryDate);
            Assert.AreEqual(quantity, batch.Quantity);
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
    }
}
