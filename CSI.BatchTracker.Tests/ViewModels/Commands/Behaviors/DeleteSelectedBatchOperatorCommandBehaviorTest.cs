using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Tests.TestHelpers.NativeModels;
using CSI.BatchTracker.ViewModels.Commands;
using NUnit.Framework;
using System;

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

        [Test]
        public void CommandCanNotExecuteIfOperatorBelongsToReceivedBatch()
        {
            ReceivedBatchTestHelper helper = new ReceivedBatchTestHelper(operatorSource);

            receivedBatchSource.SaveReceivedBatch(helper.GetUniqueBatch1());
            viewModel.SelectedBatchOperatorFromListBoxIndex = 0;

            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandCanNotExecuteIfOperatorBelongsToImplementedBatch()
        {
            ReceivedBatchTestHelper helper = new ReceivedBatchTestHelper(operatorSource);
            BatchOperatorTestHelper operatorHelper = new BatchOperatorTestHelper(operatorSource);
            ReceivedBatch receivedBatch = helper.GetUniqueBatch1();
            BatchOperator implementingOperator = operatorHelper.GetJohnDoeOperator();

            receivedBatchSource.SaveReceivedBatch(receivedBatch);
            implementedBatchSource.AddBatchToImplementationLedger(receivedBatch.BatchNumber, DateTime.Now, operatorSource.FindBatchOperator(2));
            viewModel.SelectedBatchOperatorFromListBoxIndex = 1;

            Assert.False(command.CanExecute(null));
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
