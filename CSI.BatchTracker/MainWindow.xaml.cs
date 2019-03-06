﻿using CSI.BatchTracker.Domain;
using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.DataSource.MemorySource;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Experimental;
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public DataStore DataStore { get; set; }
        public MemoryDataSource Repository { get; set; }
        public IActiveInventorySource InventorySource { get { return inventorySource; } set { inventorySource = value; } }
        public ObservableCollection<InventoryBatch> CurrentInventory { get; private set; }

        IReceivedBatchSource receivingSource;
        IBatchOperatorSource operatorSource;
        IActiveInventorySource inventorySource;

        MemoryStoreContext Store { get; set; }
        BatchOperatorViewModel batchOperatorViewModel;
        ReceivingManagementViewModel receivingManagmentViewModel;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            Store = new MemoryStoreContext();

            DataStore = new DataStore();
            Repository = new MemoryDataSource(DataStore, Store);
            SetupBatchOperators();
            SetupColors();
            //SetupInventory();
            InitializeComponent();
            DataContext = this;
            MemoryStoreContext context = new MemoryStoreContext();
            
            operatorSource = new MemoryBatchOperatorSource(context);
            inventorySource = new MemoryActiveInventorySource(context);
            receivingSource = new MemoryReceivedBatchSource(context, inventorySource);
            CurrentInventory = inventorySource.CurrentInventory;

            AddBatchOperatorsToRepo(operatorSource);

            batchOperatorViewModel = new BatchOperatorViewModel(Repository);
            receivingManagmentViewModel = new ReceivingManagementViewModel(
                new DuracolorIntermixBatchNumberValidator(),
                new DuracolorIntermixColorList(),
                receivingSource,
                operatorSource,
                inventorySource
            );
            //BatchOperatorManagementWindow window = new BatchOperatorManagementWindow(batchOperatorViewModel);
            inventorySource.AddReceivedBatchToInventory(new ReceivedBatch("Yellow", "872880703401", DateTime.Parse("6/7/2018"), 4, 42089, GetRandomOperatorFromRepository()));
            BatchReceivingManagementWindow window = new BatchReceivingManagementWindow(receivingManagmentViewModel);

            window.ShowDialog();
            NotifyPropertyChanged("CurrentInventory");
            
        }

        void AddBatchOperatorsToRepo(IBatchOperatorSource source)
        {
            source.SaveOperator(new BatchOperator("Jane", "Doe"));
            source.SaveOperator(new BatchOperator("John", "Roe"));
        }

        void SetupBatchOperators()
        {
            List<BatchOperator> batchOperators = new List<BatchOperator>
            {
                new BatchOperator("Lang", "Roubadoux"),
                new BatchOperator("Nicholas", "Huron"),
                new BatchOperator("Wally", "McTalian")
            };

            foreach (BatchOperator batchOperator in batchOperators)
            {
                Entity<BatchOperator> entity;
                ITransaction adder;

                entity = new Entity<BatchOperator>(batchOperator);
                adder = new AddBatchOperatorTransaction(entity, Store);
                adder.Execute();
            }
        }

        void SetupColors()
        {
            ObservableCollection<string> colors = new ObservableCollection<string>
            {
                "White", "Black", "Yellow", "Red", "Blue Red", "Bright Red", "Bright Yellow", "Deep Green", "Deep Blue"
            };

            DataStore.Colors = colors;
        }

        void SetupInventory()
        {
            Random random = new Random();
            Repository.ReceiveInventory(new ReceivedBatch("White", "872881103201", DateTime.Parse("5/21/2018"), 6, 42018, GetRandomOperatorFromRepository()));
            Repository.ReceiveInventory(new ReceivedBatch("White", "872881103201", DateTime.Parse("5/21/2018"), 2, 42018, GetRandomOperatorFromRepository()));
            Repository.ReceiveInventory(new ReceivedBatch("Black", "872881503204", DateTime.Parse("5/21/2018"), 3, 42018, GetRandomOperatorFromRepository()));
            Repository.ReceiveInventory(new ReceivedBatch("Yellow", "872880703401", DateTime.Parse("5/25/2018"), 5, 42018, GetRandomOperatorFromRepository()));
            Repository.ReceiveInventory(new ReceivedBatch("White", "872881501703", DateTime.Parse("6/1/2018"), 8, 42033, GetRandomOperatorFromRepository()));
            Repository.ReceiveInventory(new ReceivedBatch("Red", "872880404201", DateTime.Parse("6/1/2018"), 2, 42033, GetRandomOperatorFromRepository()));
            Repository.ReceiveInventory(new ReceivedBatch("Blue Red", "872880901304", DateTime.Parse("6/1/2018"), 4, 42033, GetRandomOperatorFromRepository()));
            Repository.ReceiveInventory(new ReceivedBatch("Bright Red", "872880101206", DateTime.Parse("6/1/2018"), 1, 42033, GetRandomOperatorFromRepository()));
            Repository.ReceiveInventory(new ReceivedBatch("Deep Blue", "872881305103", DateTime.Parse("6/7/2018"), 1, 42084, GetRandomOperatorFromRepository()));
            Repository.ReceiveInventory(new ReceivedBatch("Deep Green", "872880803205", DateTime.Parse("6/7/2018"), 3, 42084, GetRandomOperatorFromRepository()));
            Repository.ReceiveInventory(new ReceivedBatch("Bright Yellow", "872880506701", DateTime.Parse("6/7/2018"), 2, 42084, GetRandomOperatorFromRepository()));
            Repository.ReceiveInventory(new ReceivedBatch("Yellow", "872880703401", DateTime.Parse("6/7/2018"), 4, 42089, GetRandomOperatorFromRepository()));
        }

        BatchOperator GetRandomOperatorFromRepository()
        {
            Random random = new Random();
            int index = random.Next(1, Store.BatchOperators.Count + 1);
            Entity<BatchOperator> entity = Store.BatchOperators[index];

            return entity.NativeModel;
        }

        private void AddOperator(object sender, RoutedEventArgs e)
        {
            Repository.SaveOperator(new BatchOperator(operatorFN.Text, operatorLN.Text));
        }

        private void AddInventoriedBatch(object sender, RoutedEventArgs e)
        {
            try
            {
                Repository.ReceiveInventory(
                    new ReceivedBatch(
                        batchColor.SelectedValue.ToString(),
                        ValidateBatch(batchNumber.Text),
                        (DateTime)recvDate.SelectedDate,
                        int.Parse(batchQty.Text),
                        int.Parse(poNumber.Text),
                        Store.BatchOperators[batchOperator.SelectedIndex].NativeModel
                    )
                );

                inventoryGrid.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddBatchToLedger(object sender, RoutedEventArgs e)
        {
            int targetBatch = Repository.CurrentInventoryIdMappings[ledgerBatchSelection.SelectedIndex];
            int targetOperator = Repository.BatchOperatorIdMappings[ledgerBatchOperator.SelectedIndex];
            InventoryBatch batch = Store.CurrentInventory[targetBatch].NativeModel;

            Repository.ImplementBatch(
                batch.BatchNumber, 
                (DateTime)ledgerBatchDate.SelectedDate,
                Store.BatchOperators[targetOperator].NativeModel
            );

            inventoryGrid.Items.Refresh();
            ledgerGrid.Items.Refresh();
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
    }
}
