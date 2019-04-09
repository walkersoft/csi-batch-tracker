using CSI.BatchTracker.Domain.DataSource.MemorySource;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors;
using CSI.BatchTracker.ViewModels;
using CSI.BatchTracker.ViewModels.Commands;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.WithMemoryStore
{
    [TestFixture]
    class OpenBatchHistoryViewerCommandTest : OpenBatchHistoryViewerCommandBehaviorTest
    {
        [SetUp]
        public override void SetUp()
        {
            MemoryStoreContext context = new MemoryStoreContext();
            inventorySource = new MemoryActiveInventorySource(context);
            receivedBatchSource = new MemoryReceivedBatchSource(context, inventorySource);
            implementedBatchSource = new MemoryImplementedBatchSource(context, inventorySource);
            operatorSource = new MemoryBatchOperatorSource(context);
            viewModel = new MainWindowViewModel(inventorySource, receivedBatchSource, implementedBatchSource, operatorSource);
            command = new OpenBatchHistoryViewerCommand(viewModel);
            base.SetUp();
        }
    }
}
