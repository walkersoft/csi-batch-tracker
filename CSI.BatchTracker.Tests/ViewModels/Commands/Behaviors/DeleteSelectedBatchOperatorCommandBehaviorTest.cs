﻿using CSI.BatchTracker.ViewModels.Commands;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors
{
    [TestFixture]
    abstract class DeleteSelectedBatchOperatorCommandBehaviorTest : BatchOperatorViewModelCommandTestingBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            command = new DeleteSelectedBatchOperatorCommand(viewModel);
        }

        [Test]
        public void CommandCanExecute()
        {
            PutSingleBatchOperatorIntoDataSource();
            viewModel.SelectedBatchOperatorFromListBoxIndex = 0;
            Assert.True(command.CanExecute(null));
        }

        //[Test]
        public void CommandCanNotExecute()
        {
            //Assert.False(command.CanExecute(null));
            /* TODO: once more of the persistence layer is completed and wired
             * up this test needs to be re-activated so the command can be properly
             * tested. Specifically, a BatchOperator cannot be deleted if it is
             * referenced elsewhere in the data source as the record will then
             * become orphaned to the parent.
             */
        }

        [Test]
        public void ExistingBatchOperatorGetsDeleted()
        {
            int expectedCount = 0;
            PutSingleBatchOperatorIntoDataSource();
            viewModel.SelectedBatchOperatorFromListBoxIndex = 0;
            command.Execute(null);
            Assert.AreEqual(expectedCount, viewModel.OperatorRepository.Count);
        }

        void PutSingleBatchOperatorIntoDataSource()
        {
            viewModel.FirstName = "Jane";
            viewModel.LastName = "Doe";
            viewModel.PersistBatchOperator();
        }
    }
}