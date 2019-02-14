using CSI.BatchTracker.Domain;
using CSI.BatchTracker.Domain.DataSource.MemorySource;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.ViewModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            viewModel = new ReceivingManagementViewModel(
                new DuracolorIntermixBatchNumberValidator(),
                new DuracolorIntermixColorList(),
                new MemoryReceivedBatchSource(context),
                new MemoryBatchOperatorSource(context)
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
