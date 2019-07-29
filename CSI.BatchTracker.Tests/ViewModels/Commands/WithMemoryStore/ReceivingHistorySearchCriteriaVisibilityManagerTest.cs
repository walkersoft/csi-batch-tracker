﻿using CSI.BatchTracker.Domain;
using CSI.BatchTracker.Domain.DataSource.MemorySource;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Tests.ViewModels.Commands.Behaviors;
using CSI.BatchTracker.ViewModels;
using CSI.BatchTracker.ViewModels.Commands;
using NUnit.Framework;

namespace CSI.BatchTracker.Tests.Domain
{
    [TestFixture]
    class ReceivingHistorySearchCriteriaVisibilityManagerTest : ReceivingHistorySearchCriteriaVisibilityManagerCommandBehaviorTest
    {
        [SetUp]
        public void SetUp()
        {
            MemoryStoreContext context = new MemoryStoreContext();
            operatorSource = new MemoryBatchOperatorSource(context);
            inventorySource = new MemoryActiveInventorySource(context);
            receivedBatchSource = new MemoryReceivedBatchSource(context, inventorySource);
            implementedBatchSource = new MemoryImplementedBatchSource(context, inventorySource);
            viewModel = new ReceivingHistoryViewModel(
                receivedBatchSource,
                inventorySource,
                operatorSource,
                implementedBatchSource,
                GetReceivingManagementViewModel()
            );
            command = new ChangeSearchCriteriaPanelVisibilityCommand(viewModel);
        }

        ReceivingManagementViewModel GetReceivingManagementViewModel()
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
