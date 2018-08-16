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
    class BatchOperatorRepositoryTest
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

        [Test]
        public void GetAllOperatorEntites()
        {
            int expectedQty = 3;
            LoadThreeOperatorsIntoRepository();

            Assert.AreEqual(expectedQty, repository.FindAll().Count);
        }

        [Test]
        public void EmptyResultsGivenIfIdDoesNotExistInStore()
        {
            int expectedQty = 0;
            Assert.AreEqual(expectedQty, repository.FindById(1).Count);
        }

        [Test]
        public void FindSetAmountOfEntities()
        {
            int expectedQty = 2;
            LoadThreeOperatorsIntoRepository();
            Assert.AreEqual(expectedQty, repository.FindAll(2).Count);
        }

        [Test]
        public void DeleteBatchOperatorEntityById()
        {
            int expectedQty = 2;
            int targetId = 1;
            LoadThreeOperatorsIntoRepository();

            repository.Delete(targetId);

            Assert.AreEqual(expectedQty, repository.FindAll().Count);
        }

        [Test]
        public void NoChangeInRepositoryWhenDeletingNonExistantId()
        {
            int expectedQty = 3;
            int targetId = 0;
            LoadThreeOperatorsIntoRepository();

            Assert.AreEqual(expectedQty, repository.FindAll().Count);

            repository.Delete(targetId);

            Assert.AreEqual(expectedQty, repository.FindAll().Count);
        }

        List<BatchOperator> LoadThreeOperatorsIntoRepository()
        {
            List<BatchOperator> operators = new List<BatchOperator>()
            {
                new BatchOperator("Jane", "Doe"),
                new BatchOperator("Jane", "Doe"),
                new BatchOperator("Jane", "Doe")
            };

            for (int i = 0; i < operators.Count; ++i)
            {
                repository.Save(new Entity<BatchOperator>(i + 1, operators[i]));
            }

            return operators;
        }
    }
}
