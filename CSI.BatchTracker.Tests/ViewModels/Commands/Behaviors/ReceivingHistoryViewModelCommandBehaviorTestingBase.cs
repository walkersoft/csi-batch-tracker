using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.ViewModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors
{
    [TestFixture]
    abstract class ReceivingHistoryViewModelCommandBehaviorTestingBase
    {
        protected ICommand command;
        protected IReceivedBatchSource receivedBatchSource;
        protected IActiveInventorySource inventorySource;
        protected ReceivingHistoryViewModel viewModel;
    }
}
