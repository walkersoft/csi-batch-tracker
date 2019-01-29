using CSI.BatchTracker.Domain;
using CSI.BatchTracker.Domain.Contracts;
using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.DataSource.MemorySource;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Tests.TestHelpers.NativeModels;
using CSI.BatchTracker.ViewModels;
using CSI.BatchTracker.ViewModels.Commands;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CSI.BatchTracker.Tests.ViewModels.Commands
{
    [TestFixture]
    class RemoveReceivableBatchFromSessionLedgerCommandTest : ReceivingManagementViewModelCommandTestingBase
    {        
        ReceivedBatchTestHelper helper;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            helper = new ReceivedBatchTestHelper();
            viewModel = new ReceivingManagementViewModel(validator, colorList, operatorSource);
            command = new RemoveReceivableBatchFromSessionLedgerCommand(viewModel);
        }

        [Test]
        public void UnableToExecuteWhenSessionLedgerIsEmpty()
        {
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void UnableToExecuteIfSessionLedgerDoesNotHaveItemSelected()
        {
            AddItemToSessionLedger();
            Assert.False(command.CanExecute(null));
        }

        void AddItemToSessionLedger()
        {
            SetupValidReceivedBatchInViewModel();
            InjectTwoOperatorsIntoRepository();
            ICommand addEntry = new AddReceivedBatchToReceivingSessionLedgerCommand(viewModel);
            addEntry.Execute(null);
        }

        [Test]
        public void AbleToExecuteIfEntryIsInSessionLedgerAndEntryIsSelected()
        {
            AddItemToSessionLedger();
            viewModel.SessionLedgerSelectedItem = 0;
            Assert.True(command.CanExecute(null));
        }

        [Test]
        public void EntryIsRemovedFromSessionLedgerSuccessfully()
        {
            int expectedCountBefore = 1;
            int expectedCountAfter = 0;
            AddItemToSessionLedger();

            Assert.AreEqual(expectedCountBefore, viewModel.SessionLedger.Count);

            viewModel.SessionLedgerSelectedItem = 0;
            command.Execute(null);

            Assert.AreEqual(expectedCountAfter, viewModel.SessionLedger.Count);
        }
    }
}
