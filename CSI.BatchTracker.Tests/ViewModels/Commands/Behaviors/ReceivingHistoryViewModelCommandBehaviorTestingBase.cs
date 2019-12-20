using CSI.BatchTracker.Domain;
using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Tests.TestHelpers.NativeModels;
using CSI.BatchTracker.ViewModels;
using NUnit.Framework;
using System.Windows.Input;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors
{
    [TestFixture]
    abstract class ReceivingHistoryViewModelCommandBehaviorTestingBase
    {
        protected ICommand command;
        protected IReceivedBatchSource receivedBatchSource;
        protected IActiveInventorySource inventorySource;
        protected IBatchOperatorSource operatorSource;
        protected IImplementedBatchSource implementedBatchSource;
        protected ReceivingHistoryViewModel viewModel;
        protected ReceivedBatchTestHelper helper;

        [SetUp]
        public virtual void SetUp()
        {
            helper = new ReceivedBatchTestHelper(operatorSource);
        }

        protected ReceivingManagementViewModel GetReceivingManagementViewModel()
        {
            return new ReceivingManagementViewModel(
                new DuracolorIntermixBatchNumberValidator(),
                new DuracolorIntermixColorList(),
                receivedBatchSource,
                operatorSource,
                inventorySource
            );
        }
    }
}
