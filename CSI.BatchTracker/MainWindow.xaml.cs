using CSI.BatchTracker.Domain;
using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Experimental;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CSI.BatchTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DataStore DataStore { get; set; }
        BatchDataRepository repository;

        public MainWindow()
        {
            DataStore = new DataStore();
            repository = new BatchDataRepository(DataStore);
            SetupBatchOperators();
            SetupColors();
            SetupInventory();
            InitializeComponent();
            DataContext = DataStore;
        }

        void SetupBatchOperators()
        {
            repository.SaveOperator(new BatchOperator("Lang", "Roubadoux"));
            repository.SaveOperator(new BatchOperator("Nicholas", "Huron"));
            repository.SaveOperator(new BatchOperator("Wally", "McTalian"));
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
            repository.ReceiveInventory(new ReceivedBatch("White", "872881103201", DateTime.Parse("5/21/2018"), 6, 42018, DataStore.BatchOperators[random.Next(0, DataStore.BatchOperators.Count)]));
            repository.ReceiveInventory(new ReceivedBatch("White", "872881103201", DateTime.Parse("5/21/2018"), 2, 42018, DataStore.BatchOperators[random.Next(0, DataStore.BatchOperators.Count)]));
            repository.ReceiveInventory(new ReceivedBatch("Black", "872881503204", DateTime.Parse("5/21/2018"), 3, 42018, DataStore.BatchOperators[random.Next(0, DataStore.BatchOperators.Count)]));
            repository.ReceiveInventory(new ReceivedBatch("Yellow", "872880703401", DateTime.Parse("5/25/2018"), 5, 42018, DataStore.BatchOperators[random.Next(0, DataStore.BatchOperators.Count)]));
            repository.ReceiveInventory(new ReceivedBatch("White", "872881501703", DateTime.Parse("6/1/2018"), 8, 42033, DataStore.BatchOperators[random.Next(0, DataStore.BatchOperators.Count)]));
            repository.ReceiveInventory(new ReceivedBatch("Red", "872880404201", DateTime.Parse("6/1/2018"), 2, 42033, DataStore.BatchOperators[random.Next(0, DataStore.BatchOperators.Count)]));
            repository.ReceiveInventory(new ReceivedBatch("Blue Red", "872880901304", DateTime.Parse("6/1/2018"), 4, 42033, DataStore.BatchOperators[random.Next(0, DataStore.BatchOperators.Count)]));
            repository.ReceiveInventory(new ReceivedBatch("Bright Red", "872880101206", DateTime.Parse("6/1/2018"), 1, 42033, DataStore.BatchOperators[random.Next(0, DataStore.BatchOperators.Count)]));
            repository.ReceiveInventory(new ReceivedBatch("Deep Blue", "872881305103", DateTime.Parse("6/7/2018"), 1, 42084, DataStore.BatchOperators[random.Next(0, DataStore.BatchOperators.Count)]));
            repository.ReceiveInventory(new ReceivedBatch("Deep Green", "872880803205", DateTime.Parse("6/7/2018"), 3, 42084, DataStore.BatchOperators[random.Next(0, DataStore.BatchOperators.Count)]));
            repository.ReceiveInventory(new ReceivedBatch("Bright Yellow", "872880506701", DateTime.Parse("6/7/2018"), 2, 42084, DataStore.BatchOperators[random.Next(0, DataStore.BatchOperators.Count)]));
            repository.ReceiveInventory(new ReceivedBatch("Yellow", "872880703401", DateTime.Parse("6/7/2018"), 4, 42089, DataStore.BatchOperators[random.Next(0, DataStore.BatchOperators.Count)]));
        }

        private void AddOperator(object sender, RoutedEventArgs e)
        {
            repository.SaveOperator(new BatchOperator(operatorFN.Text, operatorLN.Text));
        }

        private void AddInventoriedBatch(object sender, RoutedEventArgs e)
        {
            try
            {
                repository.ReceiveInventory(
                    new ReceivedBatch(
                        batchColor.SelectedValue.ToString(),
                        ValidateBatch(batchNumber.Text), 
                        (DateTime)recvDate.SelectedDate,
                        int.Parse(batchQty.Text),
                        int.Parse(poNumber.Text),
                        DataStore.BatchOperators[batchOperator.SelectedIndex]
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
            InventoryBatch batch = DataStore.InventoryBatches[ledgerBatchSelection.SelectedIndex];

            repository.ImplementBatch(
                batch.BatchNumber, 
                (DateTime)ledgerBatchDate.SelectedDate, 
                DataStore.BatchOperators[ledgerBatchOperator.SelectedIndex]
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
