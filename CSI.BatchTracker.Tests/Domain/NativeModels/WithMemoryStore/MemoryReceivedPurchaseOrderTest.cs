using CSI.BatchTracker.Domain.DataSource.MemorySource;
using CSI.BatchTracker.Storage.MemoryStore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.Domain.NativeModels.WithMemoryStore
{
    [TestFixture]
    class MemoryReceivedPurchaseOrderTest : ReceivedPurchaseOrderTest
    {
        [SetUp]
        public override void SetUp()
        {
            operatorSource = new MemoryBatchOperatorSource(new MemoryStoreContext());
            base.SetUp();
        }
    }
}
