using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors
{
    [TestFixture]
    abstract class ReceivedBatchForEditingSelectionChangedCommandBehaviorTest : ReceivedPurchaseOrderEditorViewModelTestingBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [Test]
        public void CommandWillNotExecuteIfNoRecordIsSelected()
        {
            viewModel.ReceivedBatchesSelectedIndex = -1;
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void ExecutedCommandWillPopulateCurrentReceivedBatch()
        {
            string expectedQuantity = "5";
            int expectedColorIndex = 1;
            string expectedUpdateText = "Update Item";

            viewModel.ReceivedBatchesSelectedIndex = 1;
            command.Execute(null);

            Assert.AreEqual("Black", viewModel.Colors[viewModel.SelectedColorIndex].ToString());
            Assert.AreEqual(blackBatch, viewModel.BatchNumber);
            Assert.AreEqual(expectedQuantity, viewModel.Quantity);
            Assert.AreEqual(expectedColorIndex, viewModel.SelectedColorIndex);
            Assert.AreEqual(expectedUpdateText, viewModel.UpdateText);
        }
    }
}
