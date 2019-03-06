using CSI.BatchTracker.Domain;
using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.DataSource.MemorySource;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.ViewModels;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.ViewModels
{
    [TestFixture]
    class ReceivingManagementViewModelTest
    {
        ReceivingManagementViewModel viewModel;

        [SetUp]
        public void SetUp()
        {
            MemoryStoreContext context = new MemoryStoreContext();
            IActiveInventorySource inventorySource = new MemoryActiveInventorySource(context);
            viewModel = new ReceivingManagementViewModel(
                new DuracolorIntermixBatchNumberValidator(),
                new DuracolorIntermixColorList(),
                new MemoryReceivedBatchSource(context, inventorySource),
                new MemoryBatchOperatorSource(context),
                new MemoryActiveInventorySource(context)
            );
        }

        [Test]
        public void AttemptingToAddInvalidReceivedBatchToSessionLedgerResultsInNoChanges()
        {       
            int expectedLedgerCount = 0;
            viewModel.AddReceivedBatchToSessionLedger();

            Assert.AreEqual(expectedLedgerCount, viewModel.SessionLedger.Count);
        }

        [Test]
        public void ColorListIsInitializedUponViewModelContruction()
        {
            Assert.NotNull(viewModel.Colors);
        }
    }
}
