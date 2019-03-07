using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.ViewModels;
using CSI.BatchTracker.ViewModels.Commands;
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
    abstract public class BatchOperatorViewModelCommandTestingBase
    {
        protected IBatchOperatorSource operatorSource;
        protected ICommand command;
        protected BatchOperatorViewModel viewModel;

        [SetUp]
        public virtual void SetUp()
        {
            viewModel = new BatchOperatorViewModel(operatorSource);
        }
    }
}
