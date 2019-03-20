using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.ViewModels.Commands;
using CSI.BatchTracker.Views;
using CSI.BatchTracker.Views.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CSI.BatchTracker.ViewModels
{
    public sealed class MainWindowViewModel : ViewModelBase
    {
        public int ImplementableBatchSelectedIndex { get; set; }

        public int ImplementingBatchOperatorSelectedIndex { get; set; }
        public DateTime? ImplementationDateTime { get; set; }

        public ObservableCollection<InventoryBatch> CurrentInventory { get; private set; }
        public ObservableCollection<LoggedBatch> ImplementedBatchLedger { get; private set; }

        public IView ReceivingManagementSessionViewer { get; set; }
        public IView BatchOperatorManagementSessionViewer { get; set; }

        public ICommand LaunchReceivingManagementSessionViewerCommand { get; set; }
        public ICommand LaunchBatchOperatorManagementSessionViewerCommand { get; set; }

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

            LaunchReceivingManagementSessionViewerCommand = new OpenReceivingManagementSessionViewCommand(this);
            LaunchBatchOperatorManagementSessionViewerCommand = new OpenBatchOperatorManagementViewCommand(this);
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
            return ReceivingManagementSessionViewer != null
                && ReceivingManagementSessionViewer.CanShowView();
        }

        public void ShowReceivingManagementSessionView()
        {
            ReceivingManagementSessionViewer.ShowView();
        }

        public bool BatchOperatorManagementSessionViewIsSet()
        {
            return BatchOperatorManagementSessionViewer != null
                && BatchOperatorManagementSessionViewer.CanShowView();
        }

        public void ShowBatchOperatorManagementSessionView()
        {
            BatchOperatorManagementSessionViewer.ShowView();
        }
    }
}
