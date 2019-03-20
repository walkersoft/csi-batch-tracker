using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Views;
using CSI.BatchTracker.Views.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.ViewModels
{
    public sealed class MainWindowViewModel : ViewModelBase
    {
        public int ImplementableBatchSelectedIndex { get; set; }
        public int ImplementingBatchOperatorSelectedIndex { get; set; }
        public DateTime? ImplementationDateTime { get; set; }

        public ObservableCollection<InventoryBatch> CurrentInventory { get; private set; }
        public ObservableCollection<LoggedBatch> ImplementedBatchLedger { get; private set; }

        public IView ReceivingManagementSessionView { get; set; }

        IActiveInventorySource inventorySource;
        IReceivedBatchSource receivedBatchSource;
        IImplementedBatchSource implementedBatchSource;
        IBatchOperatorSource operatorSource;

        public MainWindowViewModel(
            IActiveInventorySource inventorySource,
            IReceivedBatchSource receivedBatchSource,
            IImplementedBatchSource implementedBatchSource,
            IBatchOperatorSource operatorSource)
        {
            this.inventorySource = inventorySource;
            this.receivedBatchSource = receivedBatchSource;
            this.implementedBatchSource = implementedBatchSource;
            this.operatorSource = operatorSource;

            AssociateInventoryAndImplementationLedgers();
            InitializeBatchImplementationSettings();
        }

        void AssociateInventoryAndImplementationLedgers()
        {
            CurrentInventory = inventorySource.CurrentInventory;
            ImplementedBatchLedger = implementedBatchSource.ImplementedBatchLedger;
        }

        void InitializeBatchImplementationSettings()
        {
            ImplementableBatchSelectedIndex = -1;
            ImplementingBatchOperatorSelectedIndex = -1;
            ImplementationDateTime = null;
        }

        public void ImplementBatchFromInventoryToImplementationLedger()
        {
            string selectedBatchNumber = GetBatchNumberOfSelectedInventoryItem();
            BatchOperator selectedBatchOperator = GetBatchOperatorFromSelectedItem();
            implementedBatchSource.AddBatchToImplementationLedger(selectedBatchNumber, (DateTime)ImplementationDateTime, selectedBatchOperator);
        }

        BatchOperator GetBatchOperatorFromSelectedItem()
        {
            return operatorSource.FindBatchOperator(operatorSource.BatchOperatorIdMappings[ImplementingBatchOperatorSelectedIndex]);
        }

        string GetBatchNumberOfSelectedInventoryItem()
        {
            return inventorySource.CurrentInventory[ImplementableBatchSelectedIndex].BatchNumber;
        }

        public bool BatchIsSetupForImplementation()
        {
            return ImplementableBatchSelectedIndex > -1
                && ImplementationDateTime != null
                && ImplementingBatchOperatorSelectedIndex > -1;
        }

        public bool ReceivingManagementSessionViewIsSet()
        {
            return ReceivingManagementSessionView != null
                && ReceivingManagementSessionView.CanShowView();
        }

        public void ShowReceivingManagementSessionView()
        {
            ReceivingManagementSessionView.ShowView();
        }
    }
}
