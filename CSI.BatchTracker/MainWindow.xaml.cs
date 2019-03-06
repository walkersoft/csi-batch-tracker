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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
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
            SetupBatchOperators();
            SetupColors();
            InitializeComponent();
            DataContext = this;
            MemoryStoreContext context = new MemoryStoreContext();
            
            operatorSource = new MemoryBatchOperatorSource(context);
            inventorySource = new MemoryActiveInventorySource(context);
            receivingSource = new MemoryReceivedBatchSource(context, inventorySource);
            CurrentInventory = inventorySource.CurrentInventory;

            AddBatchOperatorsToRepo(operatorSource);

            batchOperatorViewModel = new BatchOperatorViewModel(operatorSource);
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
    }
}
