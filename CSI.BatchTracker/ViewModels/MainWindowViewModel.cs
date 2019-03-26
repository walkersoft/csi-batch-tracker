using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.ViewModels.Commands;
using CSI.BatchTracker.Views.Contracts;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CSI.BatchTracker.ViewModels
{
    public sealed class MainWindowViewModel : ViewModelBase
    {
        IActiveInventorySource inventorySource;
        IReceivedBatchSource receivedBatchSource;
        IImplementedBatchSource implementedBatchSource;
        IBatchOperatorSource operatorSource;

        public IView ReceivingManagementSessionViewer { get; set; }
        public IView BatchOperatorManagementSessionViewer { get; set; }

        public ICommand LaunchReceivingManagementSessionViewerCommand { get; private set; }
        public ICommand LaunchBatchOperatorManagementSessionViewerCommand { get; private set; }
        public ICommand CommitInventoryBatchToImplementationLedgerCommand { get; private set; }
        public ICommand UndoSelectedImplementedBatchCommand { get; private set; }

        public int ImplementableBatchSelectedIndex { get; set; }
        public int ImplementedBatchSelectedIndex { get; set; }
        public int ImplementingBatchOperatorSelectedIndex { get; set; }
        public DateTime? ImplementationDateTime { get; set; }
        public ObservableCollection<LoggedBatch> ImplementedBatchLedger { get; private set; }
        public ObservableCollection<BatchOperator> OperatorRepository { get; private set; }
        public ObservableCollection<InventoryBatch> CurrentInventory { get; private set; }        

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

            AssociateCollectionsAndRepositories();
            InitializeBatchImplementationSettings();

            LaunchReceivingManagementSessionViewerCommand = new OpenReceivingManagementSessionViewCommand(this);
            LaunchBatchOperatorManagementSessionViewerCommand = new OpenBatchOperatorManagementViewCommand(this);
            CommitInventoryBatchToImplementationLedgerCommand = new CommitBatchToImplementationLedgerCommand(this);
            UndoSelectedImplementedBatchCommand = new UndoImplementedBatchCommand(this);
        }

        void AssociateCollectionsAndRepositories()
        {
            CurrentInventory = inventorySource.CurrentInventory;
            ImplementedBatchLedger = implementedBatchSource.ImplementedBatchLedger;
            OperatorRepository = operatorSource.OperatorRepository;
        }

        void InitializeBatchImplementationSettings()
        {
            ImplementableBatchSelectedIndex = -1;
            ImplementedBatchSelectedIndex = -1;
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

        public bool SelectedBatchCanBeUndone()
        {
            return ImplementedBatchLedger.Count > 0
                && ImplementedBatchSelectedIndex > -1;
        }

        public void UndoSelectedBatchFromImplementationLedger()
        {
            int targetId = implementedBatchSource.ImplementedBatchIdMappings[ImplementedBatchSelectedIndex];
            implementedBatchSource.UndoImplementedBatch(targetId);
        }
    }
}
