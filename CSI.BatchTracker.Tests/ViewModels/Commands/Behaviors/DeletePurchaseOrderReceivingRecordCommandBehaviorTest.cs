﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors
{
    [TestFixture]
    abstract class DeletePurchaseOrderReceivingRecordCommandBehaviorTest : ReceivedPurchaseOrderEditorViewModelTestingBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [Test]
        public void CommandWillNotExecuteIfNoBatchIsSelectedInReceivedBatchesLedger()
        {
            viewModel.ReceivedBatchesSelectedIndex = -1;
            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillNotExecuteIfSelectedBatchForDeletionHasBeenImplemented()
        {
            string batchNumber = viewModel.ReceivedBatches[0].BatchNumber;
            implementedBatchSource.AddBatchToImplementationLedger(batchNumber, DateTime.Now, operatorSource.FindBatchOperator(1));
            viewModel.ReceivedBatchesSelectedIndex = 0;

            Assert.False(command.CanExecute(null));
        }

        [Test]
        public void CommandWillExecuteIfSelectedBatchHasNotBeenImplemented()
        {
            viewModel.ReceivedBatchesSelectedIndex = 0;
            Assert.True(command.CanExecute(null));
        }

        [Test]
        public void ExecutedCommandWillRemoveTheBatchFromTheReceivingLedger()
        {
            int expectedCount = 2;

            command.Execute(null);

            Assert.AreEqual(expectedCount, viewModel.ReceivedBatches.Count);
        }
    }
}