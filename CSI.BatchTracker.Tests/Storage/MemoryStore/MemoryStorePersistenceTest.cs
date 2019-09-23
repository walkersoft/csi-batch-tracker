using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Storage.Contracts;
using CSI.BatchTracker.Storage.MemoryStore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CSI.BatchTracker.Domain.DataSource.MemorySource;
using CSI.BatchTracker.Domain.NativeModels;

namespace CSI.BatchTracker.Tests.Storage.MemoryStore
{
    [TestFixture]
    class MemoryStorePersistenceTest
    {
        IPersistenceManager<MemoryStoreContext> persistenceManager;
        MemoryStoreContext context;
        string contextLocation;
        IBatchOperatorSource operatorSource;
        IActiveInventorySource inventorySource;
        IReceivedBatchSource receivedBatchSource;
        IImplementedBatchSource implementedBatchSource;

        [SetUp]
        public void SetUp()
        {
            contextLocation = Path.GetTempPath() + "MemoryStoreTest.store";
            context = new MemoryStoreContext();
            operatorSource = new MemoryBatchOperatorSource(context);
            inventorySource = new MemoryActiveInventorySource(context);
            receivedBatchSource = new MemoryReceivedBatchSource(context, inventorySource);
            implementedBatchSource = new MemoryImplementedBatchSource(context, inventorySource);
            persistenceManager = new MemoryStorePersistenceManager(context, contextLocation);
        }

        [TearDown]
        public void TearDown()
        {
            File.Delete(contextLocation);
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
            persistenceManager.Context = new MemoryStoreContext();

            Assert.AreEqual(expectedRepoBeforeCount, persistenceManager.Context.BatchOperators.Count);

            persistenceManager.LoadDataSource();

            Assert.AreEqual(expectedRepoAfterCount, persistenceManager.Context.BatchOperators.Count);
        }
    }
}
