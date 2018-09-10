using CSI.BatchTracker.Contracts;
using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.DataSource.Repositories;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Experimental;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.Domain.DataSource
{
    [TestFixture]
    class InventoryBatchRepositoryTest
    {
        IRepository<Entity<InventoryBatch>> repository;

        [SetUp]
        public void SetUp()
        {
            repository = new InventoryBatchRepository(new DataStore());
        }

        [Test]
        public void SaveNewEntityInStore()
        {
            int expectedQty = 1;
            InventoryBatch batch = NewInventoryBatch();
            Entity<InventoryBatch> entity = new Entity<InventoryBatch>(batch);

            repository.Save(entity);
            List<Entity<InventoryBatch>> list = repository.FindById(1);

            Assert.AreEqual(expectedQty, list.Count);
            Assert.AreSame(batch, list[0].NativeModel);
        }

        InventoryBatch NewInventoryBatch()
        {
            return new InventoryBatch("Yellow", "872882301205", DateTime.Now, 1);
        }
    }
}
