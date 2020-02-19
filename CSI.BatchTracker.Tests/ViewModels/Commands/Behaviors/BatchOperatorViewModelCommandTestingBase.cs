using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.ViewModels;
using NUnit.Framework;
using System.Windows.Input;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors
{
    [TestFixture]
    abstract public class BatchOperatorViewModelCommandTestingBase
    {
        protected IBatchOperatorSource operatorSource;
        protected IReceivedBatchSource receivedBatchSource;
        protected IImplementedBatchSource implementedBatchSource;
        protected IActiveInventorySource inventorySource;
        protected ICommand command;
        protected BatchOperatorViewModel viewModel;

        [SetUp]
        public virtual void SetUp()
        {
            viewModel = new BatchOperatorViewModel(operatorSource);
        }
    }
}
