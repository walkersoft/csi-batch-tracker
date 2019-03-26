using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Tests.TestHelpers.NativeModels;
using CSI.BatchTracker.ViewModels;
using NUnit.Framework;
using System;
using System.Windows.Input;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors
{
    [TestFixture]
    abstract class MainWindowViewModelCommandBehaviorTestingBase
    {
        protected ICommand command;
        protected IBatchOperatorSource operatorSource;
        protected IActiveInventorySource inventorySource;
        protected IReceivedBatchSource receivedBatchSource;
        protected IImplementedBatchSource implementedBatchSource;
        protected MainWindowViewModel viewModel;

        protected BatchOperatorTestHelper operatorHelper;

        [SetUp]
        public virtual void SetUp()
        {
            operatorHelper = new BatchOperatorTestHelper();
        }

        protected ReceivedBatch SetupReceivedBatchWithQuantityOfTwo()
        {
            return new ReceivedBatch("Yellow", "872880206202", DateTime.Now, 2, 55555, operatorHelper.GetJaneDoeOperator());
        }

        protected void SetupInventoryStateToImplementBatch()
        {
            operatorSource.SaveOperator(operatorHelper.GetJaneDoeOperator());
            operatorSource.SaveOperator(operatorHelper.GetJohnDoeOperator());
            receivedBatchSource.SaveReceivedBatch(SetupReceivedBatchWithQuantityOfTwo());
        }

        protected string SetupInventoryStateAndReceiveSingleBatchAndReturnBatchNumber()
        {
            ReceivedBatch batch = SetupReceivedBatchWithQuantityOfTwo();
            SetupInventoryStateToImplementBatch();
            implementedBatchSource.AddBatchToImplementationLedger(batch.BatchNumber, DateTime.Now, batch.ReceivingOperator);

            return batch.BatchNumber;
        }
    }
}
