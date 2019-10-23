using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.DataSource.MemorySource;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Storage.Contracts;
using CSI.BatchTracker.Storage.MemoryStore;
using NUnit.Framework;
using System;
using System.IO;

namespace CSI.BatchTracker.Tests.Storage.MemoryStore
{
    [TestFixture]
    class MemoryStorePersistenceTest
    {
        IPersistenceManager<MemoryStoreContext> persistenceManager;
        string contextLocation;
        IBatchOperatorSource operatorSource;
        IActiveInventorySource inventorySource;
        IReceivedBatchSource receivedBatchSource;
        IImplementedBatchSource implementedBatchSource;

        [SetUp]
        public void SetUp()
        {
            contextLocation = Path.GetTempPath() + "MemoryStoreTest.store";
            persistenceManager = new MemoryStorePersistenceManager(contextLocation);
            operatorSource = new MemoryBatchOperatorSource(persistenceManager.Context);
            inventorySource = new MemoryActiveInventorySource(persistenceManager.Context);
            receivedBatchSource = new MemoryReceivedBatchSource(persistenceManager.Context, inventorySource);
            implementedBatchSource = new MemoryImplementedBatchSource(persistenceManager.Context, inventorySource);
        }

        [TearDown]
        public void TearDown()
        {
            File.Delete(contextLocation);
        }

        [Test]
        public void PersistenceManagerCanBeSetupEmpty()
        {
            persistenceManager = new MemoryStorePersistenceManager();

            Assert.IsEmpty(persistenceManager.StoredContextLocation);
            Assert.IsNotNull(persistenceManager.Context);
        }

        [Test]
        public void DataSourceCanBeSavedToDisk()
        {
            Assert.False(File.Exists(persistenceManager.StoredContextLocation));
            persistenceManager.SaveDataSource();
            Assert.True(File.Exists(persistenceManager.StoredContextLocation));
        }

        [Test]
        public void DataSourceWithSingleRecordInSingleRepositoryCanBeSavedAndRecalled()
        {
            int expectedRepoBeforeCount = 0;
            int expectedRepoAfterCount = 1;

            operatorSource.SaveOperator(new BatchOperator("Jane", "Doe"));
            persistenceManager.SaveDataSource();
            persistenceManager.ClearDataSource();

            Assert.AreEqual(expectedRepoBeforeCount, persistenceManager.Context.BatchOperators.Count);

            persistenceManager.LoadDataSource();

            Assert.AreEqual(expectedRepoAfterCount, persistenceManager.Context.BatchOperators.Count);
        }

        [Test]
        public void DataSourceWithSingleRecordInAllRepositoriesCanBeSavedAndRecalled()
        {
            int expectedRepoBeforeCount = 0;
            int expectedRepoAfterCount = 1;

            operatorSource.SaveOperator(new BatchOperator("Jane", "Doe"));
            receivedBatchSource.SaveReceivedBatch(new ReceivedBatch("White", "872881111111", DateTime.Now, 5, 11111, operatorSource.FindBatchOperator(1)));
            implementedBatchSource.AddBatchToImplementationLedger("872881111111", DateTime.Now, operatorSource.FindBatchOperator(1));
            persistenceManager.SaveDataSource();
            persistenceManager.ClearDataSource();

            Assert.AreEqual(expectedRepoBeforeCount, persistenceManager.Context.BatchOperators.Count);
            Assert.AreEqual(expectedRepoBeforeCount, persistenceManager.Context.ReceivingLedger.Count);
            Assert.AreEqual(expectedRepoBeforeCount, persistenceManager.Context.ImplementedBatchLedger.Count);
            Assert.AreEqual(expectedRepoBeforeCount, persistenceManager.Context.CurrentInventory.Count);

            persistenceManager.LoadDataSource();

            Assert.AreEqual(expectedRepoAfterCount, persistenceManager.Context.BatchOperators.Count);
            Assert.AreEqual(expectedRepoAfterCount, persistenceManager.Context.ReceivingLedger.Count);
            Assert.AreEqual(expectedRepoAfterCount, persistenceManager.Context.ImplementedBatchLedger.Count);
            Assert.AreEqual(expectedRepoAfterCount, persistenceManager.Context.CurrentInventory.Count);
        }
    }
}
