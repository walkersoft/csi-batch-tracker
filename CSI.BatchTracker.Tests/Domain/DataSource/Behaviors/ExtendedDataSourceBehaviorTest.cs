using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Tests.TestHelpers.NativeModels;
using CSI.BatchTracker.ViewModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.Domain.DataSource.Behaviors
{
    [TestFixture]
    class ExtendedDataSourceBehaviorTest
    {
        IBatchOperatorSource operatorSource;
        IActiveInventorySource inventorySource;
        IReceivedBatchSource receivedBatchSource;
        IImplementedBatchSource implementedBatchSource;
        BatchOperatorTestHelper batchOperatorTestHelper;
        ReceivingManagementViewModel receivingManagementViewModel;
        
        [SetUp]
        public void SetUp()
        {
            batchOperatorTestHelper = new BatchOperatorTestHelper();
        }

        [Test]
        public void TestReceivingAndInventoryMergingLedgersForConsistency()
        {

        }

        void AddFirstPurchaseOrder()
        {

        }
    }
}
