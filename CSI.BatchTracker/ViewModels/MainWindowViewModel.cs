using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.ViewModels.Commands;
using CSI.BatchTracker.Views.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
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
        public IView ReceivingHistorySessionViewer { get; set; }
        public IView ConnectedBatchInquiryViewer { get; set; }
        public IBatchHistoryView BatchHistoryViewer { get; set; }

        public ICommand LaunchReceivingManagementSessionViewerCommand { get; private set; }
        public ICommand LaunchBatchOperatorManagementSessionViewerCommand { get; private set; }
        public ICommand LaunchBatchHistoryViewerCommand { get; private set; }
        public ICommand LaunchBatchHistoryViewerWithBatchNumberCommand { get; private set; }
        public ICommand LaunchReceivingHistorySessionViewerCommand { get; private set; }
        public ICommand LaunchConnectedBatchInquiryViewerCommand { get; private set; }
        public ICommand CommitInventoryBatchToImplementationLedgerCommand { get; private set; }
        public ICommand UndoSelectedImplementedBatchCommand { get; private set; }
        public ICommand AutoBackupToggleCommand { get; private set; }

        public string WindowTitle { get; set; }
        public int ImplementableBatchSelectedIndex { get; set; }
        public int ImplementedBatchSelectedIndex { get; set; }
        public int ImplementingBatchOperatorSelectedIndex { get; set; }
        public bool AutoBackupToggleState { get; set; }
        public DateTime? ImplementationDateTime { get; set; }
        public ObservableCollection<LoggedBatch> ImplementedBatchLedger { get; private set; }
        public ObservableCollection<BatchOperator> OperatorRepository { get; private set; }
        public ObservableCollection<InventoryBatch> CurrentInventory { get; private set; }
        public ObservableCollection<AverageBatch> AverageBatchList { get; private set; }

        public int TotalInventoryCount
        {
            get { return inventorySource.TotalInventoryCount; }
        }

        public MainWindowViewModel(
            IActiveInventorySource inventorySource,
            IReceivedBatchSource receivedBatchSource,
            IImplementedBatchSource implementedBatchSource,
            IBatchOperatorSource operatorSource
        )
        {
            this.inventorySource = inventorySource;
            this.receivedBatchSource = receivedBatchSource;
            this.implementedBatchSource = implementedBatchSource;
            this.operatorSource = operatorSource;

            AssociateCollectionsAndRepositories();
            InitializeBatchImplementationSettings();

            LaunchReceivingManagementSessionViewerCommand = new OpenReceivingManagementSessionViewCommand(this);
            LaunchBatchOperatorManagementSessionViewerCommand = new OpenBatchOperatorManagementViewCommand(this);
            LaunchBatchHistoryViewerCommand = new OpenBatchHistoryViewerCommand(this);
            LaunchBatchHistoryViewerWithBatchNumberCommand = new OpenBatchHistoryViewerWithBatchNumberCommand(this);
            LaunchReceivingHistorySessionViewerCommand = new OpenReceivingHistorySessionViewCommand(this);
            LaunchConnectedBatchInquiryViewerCommand = new OpenConnectedBatchInquiryViewCommand(this);
            CommitInventoryBatchToImplementationLedgerCommand = new CommitBatchToImplementationLedgerCommand(this);
            UndoSelectedImplementedBatchCommand = new UndoImplementedBatchCommand(this);
            AutoBackupToggleCommand = new AutoBackupToggleCommand();
            AutoBackupToggleState = Properties.Settings.Default.AutoDatabaseBackup;
        }

        void InitializeBatchImplementationSettings()
        {
            ImplementableBatchSelectedIndex = -1;
            ImplementedBatchSelectedIndex = -1;
            ImplementingBatchOperatorSelectedIndex = -1;
            ImplementationDateTime = null;
        }

        public void AssociateCollectionsAndRepositories()
        {
            inventorySource.UpdateActiveInventory();
            CurrentInventory = inventorySource.CurrentInventory;
            implementedBatchSource.UpdateImplementationLedger();
            ImplementedBatchLedger = implementedBatchSource.ImplementedBatchLedger;
            OperatorRepository = operatorSource.OperatorRepository;
            UpdateAverageBatchList();
            NotifyPropertyChanged("CurrentInventory");
            NotifyPropertyChanged("ImplementedBatchLedger");
            NotifyPropertyChanged("OperatorRepository");
            NotifyPropertyChanged("TotalInventoryCount");
        }

        void UpdateAverageBatchList()
        {
            List<AverageBatch> sortedAverage = SortMergedListOfAverageBatches(GetMergedListOfAverageBatchesInImplementationLedger());
            AverageBatchList = new ObservableCollection<AverageBatch>(sortedAverage);
            NotifyPropertyChanged("AverageBatchList");
        }

        List<AverageBatch> SortMergedListOfAverageBatches(List<AverageBatch> mergedAverages)
        {
            for (int i = 0; i < mergedAverages.Count; i++)
            {
                for (int j = 0; j < mergedAverages.Count - 1; j++)
                {
                    if (mergedAverages[j].AverageUsage < mergedAverages[j + 1].AverageUsage)
                    {
                        AverageBatch swap = mergedAverages[j + 1];
                        mergedAverages[j + 1] = mergedAverages[j];
                        mergedAverages[j] = swap;
                    }
                }
            }

            return mergedAverages;
        }

        List<AverageBatch> GetMergedListOfAverageBatchesInImplementationLedger()
        {
            List<string> colors = new List<string>() { "White", "Black", "Yellow", "Red", "Blue Red", "Deep Green", "Deep Blue", "Bright Red", "Bright Yellow" };
            List<AverageBatch> averages = new List<AverageBatch>();

            if (ImplementedBatchLedger.Count > 0)
            {
                TimeSpan timeSpan = ImplementedBatchLedger[ImplementedBatchLedger.Count - 1].ActivityDate.Subtract(ImplementedBatchLedger[0].ActivityDate);

                foreach (string color in colors)
                {
                    int total = 0;

                    foreach (LoggedBatch loggedBatch in ImplementedBatchLedger)
                    {
                        if (loggedBatch.ColorName == color)
                        {
                            total++;
                        }
                    }

                    averages.Add(new AverageBatch(color, Math.Abs(timeSpan.Days), total));
                }
            }

            return averages;
        }

        public void ImplementBatchFromInventoryToImplementationLedger()
        {
            string selectedBatchNumber = GetBatchNumberOfSelectedInventoryItem();
            BatchOperator selectedBatchOperator = GetBatchOperatorFromSelectedItem();
            implementedBatchSource.AddBatchToImplementationLedger(selectedBatchNumber, (DateTime)ImplementationDateTime, selectedBatchOperator);
            UpdateAverageBatchList();
            NotifyPropertyChanged("TotalInventoryCount");
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
                && ImplementationDateTime.Value <= DateTime.Now
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
            AssociateCollectionsAndRepositories();
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

        public bool BatchHistoryViewerIsSet()
        {
            return BatchHistoryViewer != null
                && BatchHistoryViewer.CanShowView();
        }

        public void ShowBatchHistoryViewer()
        {
            BatchHistoryViewer.ShowView();
            AssociateCollectionsAndRepositories();
        }

        public bool BatchHistoryViewerIsSetAndImplementedBatchIsSelected()
        {
            return ImplementedBatchSelectedIndex > -1
                && ImplementedBatchLedger.Count > 0
                && BatchHistoryViewerIsSet();
        }

        public void ShowBatchHistoryViewerWithBatchNumber()
        {
            BatchHistoryViewer.IncomingBatchNumber = ImplementedBatchLedger[ImplementedBatchSelectedIndex].BatchNumber;
            BatchHistoryViewer.ShowView();
        }

        public bool ReceivingHistoryViewerIsSet()
        {
            return ReceivingHistorySessionViewer != null
                && ReceivingHistorySessionViewer.CanShowView();
        }

        public void ShowReceivingHistoryViewer()
        {
            ReceivingHistorySessionViewer.ShowView();
            AssociateCollectionsAndRepositories();
        }

        public bool ConnectedBatchViewerIsSet()
        {
            return ConnectedBatchInquiryViewer != null
                && ConnectedBatchInquiryViewer.CanShowView();
        }

        public void ShowConnectedBatchViewer()
        {
            ConnectedBatchInquiryViewer.ShowView();
        }

        [ExcludeFromCodeCoverage]
        public string VersionNumber => "v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        [ExcludeFromCodeCoverage]
        public string DataSourceName => Properties.Settings.Default.AttachedDatabase;
    }
}
