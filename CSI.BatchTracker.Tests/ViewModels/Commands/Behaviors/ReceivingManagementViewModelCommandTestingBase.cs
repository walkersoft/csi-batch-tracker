using CSI.BatchTracker.Domain;
using CSI.BatchTracker.Domain.Contracts;
using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Tests.TestHelpers.NativeModels;
using CSI.BatchTracker.ViewModels;
using CSI.BatchTracker.ViewModels.Commands;
using NUnit.Framework;
using System;
using System.Windows.Input;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors
{
    [TestFixture]
    abstract class ReceivingManagementViewModelCommandTestingBase
    {
        protected ICommand command;
        protected IBatchNumberValidator validator;
        protected IColorList colorList;
        protected IBatchOperatorSource operatorSource;
        protected IReceivedBatchSource receivingSource;
        protected IActiveInventorySource inventorySource;
        protected ReceivingManagementViewModel viewModel;
        protected BatchOperatorTestHelper operatorHelper;

        [SetUp]
        public virtual void SetUp()
        {
            validator = new DuracolorIntermixBatchNumberValidator();
            colorList = new DuracolorIntermixColorList();
            operatorHelper = new BatchOperatorTestHelper(operatorSource);
        }

        protected void SetupValidReceivedBatchInViewModel()
        {
            viewModel.PONumber = "11111";
            viewModel.ReceivingDate = DateTime.Now;
            viewModel.ReceivingOperatorComboBoxIndex = 0;
            viewModel.ColorSelectionComboBoxIndex = 0;
            viewModel.BatchNumber = "872880501302";
            viewModel.Quantity = "1";
        }

        protected void InjectTwoOperatorsIntoRepository()
        {
            operatorSource.SaveOperator(operatorHelper.GetJaneDoeOperator());
            operatorSource.SaveOperator(operatorHelper.GetJohnDoeOperator());
        }

        protected void AddReceivedBatchToSessionLedger()
        {
            ICommand command = new AddReceivedBatchToReceivingSessionLedgerCommand(viewModel);
            command.Execute(null);
        }
    }
}
