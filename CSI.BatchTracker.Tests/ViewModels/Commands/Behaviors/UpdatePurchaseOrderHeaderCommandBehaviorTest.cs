using NUnit.Framework;
using System;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors
{
    [TestFixture]
    abstract class UpdatePurchaseOrderHeaderCommandBehaviorTest : ReceivedPurchaseOrderEditorViewModelTestingBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [Test]
        public void CommandCannotExecuteIfPOIsEmpty()
        {
            viewModel.PONumber = string.Empty;
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandCannotExecuteIfReceivingDateIsNotSet()
        {
            viewModel.PONumber = originalPONumber.ToString();
            viewModel.ReceivingDate = DateTime.MinValue;
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandCannotExecuteIfBatchOperatorIsNotSelected()
        {
            viewModel.PONumber = originalPONumber.ToString();
            viewModel.ReceivingDate = DateTime.Now;
            viewModel.SelectedOperatorIndex = -1;
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void ExecutedCommandWillUpdatePONumbersForReceivedBatches()
        {
            int targetPONumber = 22222;

            viewModel.PONumber = targetPONumber.ToString();
            command.Execute(null);

            Assert.AreEqual(targetPONumber, viewModel.ReceivedBatches[0].PONumber);
            Assert.AreEqual(targetPONumber, viewModel.ReceivedBatches[1].PONumber);
            Assert.AreEqual(targetPONumber, viewModel.ReceivedBatches[2].PONumber);
        }

        [Test]
        public void ExecutedCommandWillUpdateReceivingDateForReceivedBatches()
        {
            DateTime targetActivityDate = viewModel.ReceivingDate.AddDays(-1);

            viewModel.ReceivingDate = targetActivityDate;
            command.Execute(null);

            Assert.AreEqual(targetActivityDate, viewModel.ReceivedBatches[0].ActivityDate);
            Assert.AreEqual(targetActivityDate, viewModel.ReceivedBatches[1].ActivityDate);
            Assert.AreEqual(targetActivityDate, viewModel.ReceivedBatches[2].ActivityDate);
        }

        [Test]
        public void ExecutedCommandWillUpdateReceivingOperatorForReceivedBatches()
        {
            int targetOperatorIndex = 1;
            string targetOperatorName = "John Doe";

            viewModel.SelectedOperatorIndex = targetOperatorIndex;
            command.Execute(null);

            Assert.AreEqual(targetOperatorName, viewModel.ReceivedBatches[0].ReceivingOperator.FullName);
            Assert.AreEqual(targetOperatorName, viewModel.ReceivedBatches[1].ReceivingOperator.FullName);
            Assert.AreEqual(targetOperatorName, viewModel.ReceivedBatches[2].ReceivingOperator.FullName);
        }

        [Test]
        public void ExecutedCommandSuccessfullyLoadsCorrectIndexForCurrentBatchOperator()
        {
            int targetOperatorIndex = 0;

            command.Execute(null);

            Assert.AreEqual(targetOperatorIndex, viewModel.SelectedOperatorIndex);
        }
    }
}
