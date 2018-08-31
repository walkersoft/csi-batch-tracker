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
    }
}
