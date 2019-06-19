using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.ViewModels;
using NUnit.Framework;
using System.Windows;
using System.Windows.Input;

namespace CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors
{
    [TestFixture]
    abstract class ReceivingHistorySearchCriteriaVisibilityManagerCommandBehaviorTest
    {
        protected IReceivedBatchSource receivedBatchSource;
        protected IActiveInventorySource inventorySource;
        protected ICommand command;
        protected ReceivingHistoryViewModel viewModel;

        [Test]
        public void DefaultCommandSetupWillExecute()
        {
            Assert.True(command.CanExecute(null));
            command.Execute(null);
            Assert.AreEqual(Visibility.Visible, viewModel.VisibilityManager.DateRangeCriteriaVisibility);
            Assert.AreEqual(Visibility.Collapsed, viewModel.VisibilityManager.DatePeriodCriteriaVisibility);
            Assert.AreEqual(Visibility.Collapsed, viewModel.VisibilityManager.SpecificDateCriteriaVisibility);
            Assert.AreEqual(Visibility.Collapsed, viewModel.VisibilityManager.PONumberCriteriaVisibility);
        }

        [Test]
        public void SearchByDateRangePanelIsVisible()
        {
            viewModel.SearchCriteriaSelectedIndex = 0;
            command.Execute(null);
            Assert.AreEqual(Visibility.Visible, viewModel.VisibilityManager.DateRangeCriteriaVisibility);
            Assert.AreEqual(Visibility.Collapsed, viewModel.VisibilityManager.DatePeriodCriteriaVisibility);
            Assert.AreEqual(Visibility.Collapsed, viewModel.VisibilityManager.SpecificDateCriteriaVisibility);
            Assert.AreEqual(Visibility.Collapsed, viewModel.VisibilityManager.PONumberCriteriaVisibility);
        }

        [Test]
        public void SearchByDatePeriodPanelIsVisible()
        {
            viewModel.SearchCriteriaSelectedIndex = 1;
            command.Execute(null);
            Assert.AreEqual(Visibility.Collapsed, viewModel.VisibilityManager.DateRangeCriteriaVisibility);
            Assert.AreEqual(Visibility.Visible, viewModel.VisibilityManager.DatePeriodCriteriaVisibility);
            Assert.AreEqual(Visibility.Collapsed, viewModel.VisibilityManager.SpecificDateCriteriaVisibility);
            Assert.AreEqual(Visibility.Collapsed, viewModel.VisibilityManager.PONumberCriteriaVisibility);
        }

        [Test]
        public void SearchBySpecificDatePanelIsVisible()
        {
            viewModel.SearchCriteriaSelectedIndex = 2;
            command.Execute(null);
            Assert.AreEqual(Visibility.Collapsed, viewModel.VisibilityManager.DateRangeCriteriaVisibility);
            Assert.AreEqual(Visibility.Collapsed, viewModel.VisibilityManager.DatePeriodCriteriaVisibility);
            Assert.AreEqual(Visibility.Visible, viewModel.VisibilityManager.SpecificDateCriteriaVisibility);
            Assert.AreEqual(Visibility.Collapsed, viewModel.VisibilityManager.PONumberCriteriaVisibility);
        }

        [Test]
        public void SearchByPONumberPanelIsVisible()
        {
            viewModel.SearchCriteriaSelectedIndex = 3;
            command.Execute(null);
            Assert.AreEqual(Visibility.Collapsed, viewModel.VisibilityManager.DateRangeCriteriaVisibility);
            Assert.AreEqual(Visibility.Collapsed, viewModel.VisibilityManager.DatePeriodCriteriaVisibility);
            Assert.AreEqual(Visibility.Collapsed, viewModel.VisibilityManager.SpecificDateCriteriaVisibility);
            Assert.AreEqual(Visibility.Visible, viewModel.VisibilityManager.PONumberCriteriaVisibility);
        }
    }
}
