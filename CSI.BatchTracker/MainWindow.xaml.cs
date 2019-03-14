using CSI.BatchTracker.Domain;
using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.DataSource.MemorySource;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Storage.Contracts;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Storage.MemoryStore.Transactions.BatchOperators;
using CSI.BatchTracker.ViewModels;
using CSI.BatchTracker.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace CSI.BatchTracker
{
    public partial class MainWindow : Window
    {
        public IActiveInventorySource InventorySource { get { return inventorySource; } set { inventorySource = value; } }
        public ObservableCollection<InventoryBatch> CurrentInventory { get; private set; }

        IReceivedBatchSource receivingSource;
        IBatchOperatorSource operatorSource;
        IImplementedBatchSource implementedBatchSource;
        IActiveInventorySource inventorySource;

        MemoryStoreContext Store { get; set; }
        BatchOperatorViewModel batchOperatorViewModel;
        ReceivingManagementViewModel receivingManagementViewModel;
        BatchHistoryViewModel batchHistoryViewModel;

        public MainWindow()
        {
            SetupColors();
            InitializeComponent();
            DataContext = this;
            MemoryStoreContext context = new MemoryStoreContext();
            
            operatorSource = new MemoryBatchOperatorSource(context);
            inventorySource = new MemoryActiveInventorySource(context);
            receivingSource = new MemoryReceivedBatchSource(context, inventorySource);
            implementedBatchSource = new MemoryImplementedBatchSource(context, inventorySource);
            CurrentInventory = inventorySource.CurrentInventory;

            AddBatchOperatorsToRepo(operatorSource);
            
            
            PopulateInventoryAndImplementBatches();          
        }

        string PopulateInventoryAndImplementBatches()
        {
            string batchNumber = "872880101101";
            operatorSource.FindAllBatchOperators();
            ReceivedBatch batch = new ReceivedBatch("White", batchNumber, DateTime.Today, 4, 54023, operatorSource.OperatorRepository[0]);

            receivingSource.SaveReceivedBatch(batch);
            batch.Quantity = 2;
            batch.PONumber = 54159;
            batch.ActivityDate = batch.ActivityDate.AddDays(5);
            batch.ReceivingOperator = operatorSource.OperatorRepository[1];
            receivingSource.SaveReceivedBatch(batch);

            for (int i = 0; i < 4; i++)
            {
                implementedBatchSource.AddBatchToImplementationLedger(batchNumber, batch.ActivityDate.AddDays(i), operatorSource.OperatorRepository[2]);
            }

            return batchNumber;
        }

        void AddBatchOperatorsToRepo(IBatchOperatorSource source)
        {
            source.SaveOperator(new BatchOperator("Jane", "Doe"));
            source.SaveOperator(new BatchOperator("John", "Roe"));
            source.SaveOperator(new BatchOperator("Wally", "Beaver"));
        }

        void SetupColors()
        {
            ObservableCollection<string> colors = new ObservableCollection<string>
            {
                "White", "Black", "Yellow", "Red", "Blue Red", "Bright Red", "Bright Yellow", "Deep Green", "Deep Blue"
            };
        }

        BatchOperator GetRandomOperatorFromRepository()
        {
            Random random = new Random();
            int index = random.Next(1, Store.BatchOperators.Count + 1);
            Entity<BatchOperator> entity = Store.BatchOperators[index];

            return entity.NativeModel;
        }

        string ValidateBatch(string batchNumber)
        {
            DuracolorIntermixBatchNumberValidator validator = new DuracolorIntermixBatchNumberValidator();

            if (validator.Validate(batchNumber) == false)
            {
                throw new Exception("Batch Number is invalid");
            }

            return batchNumber;
        }

        private void Receivingbtn_Click(object sender, RoutedEventArgs e)
        {
            receivingManagementViewModel = new ReceivingManagementViewModel(
                new DuracolorIntermixBatchNumberValidator(),
                new DuracolorIntermixColorList(),
                receivingSource,
                operatorSource,
                inventorySource
            );

            BatchReceivingManagementWindow window = new BatchReceivingManagementWindow(receivingManagementViewModel);
            window.ShowDialog();
        }

        private void Historybtn_Click(object sender, RoutedEventArgs e)
        {
            batchHistoryViewModel = new BatchHistoryViewModel(
                new DuracolorIntermixBatchNumberValidator(),
                inventorySource,
                receivingSource,
                implementedBatchSource
            );

            BatchHistoryWindow window = new BatchHistoryWindow(batchHistoryViewModel);
            window.ShowDialog();
        }

        private void Operatorsbtn_Click(object sender, RoutedEventArgs e)
        {
            batchOperatorViewModel = new BatchOperatorViewModel(operatorSource);
            BatchOperatorManagementWindow window = new BatchOperatorManagementWindow(batchOperatorViewModel);
            window.ShowDialog();
        }
    }
}
