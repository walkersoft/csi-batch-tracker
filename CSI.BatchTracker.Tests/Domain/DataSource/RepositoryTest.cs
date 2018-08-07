using CSI.BatchTracker.Contracts;
using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.DataSource.Repositores;
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
    class RepositoryTest
    {
        IRepository<Entity<BatchOperator>> repository;

        [SetUp]
        public void SetUp()
        {
            repository = new BatchOperatorRepository(new DataStore());
        }

        [Test]
        public void SaveNewEntityInStore()
        {
            int expectedQty = 1;
            BatchOperator batchOperator = new BatchOperator("Jane", "Doe");
            Entity<BatchOperator> entity = new Entity<BatchOperator>(batchOperator);

            repository.Save(entity);
            List<Entity<BatchOperator>> found = repository.FindById(1);
            Assert.AreEqual(expectedQty, repository.Items.Count);
            Assert.AreSame(batchOperator, found[0].NativeModel);
        }
    }
}
