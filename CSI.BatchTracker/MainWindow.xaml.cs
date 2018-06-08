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

        public MainWindow()
        {
            DataStore = new DataStore();
            SetupBatchOperators();
            InitializeComponent();
            DataContext = DataStore;
        }

        void SetupBatchOperators()
        {
            ObservableCollection<BatchOperator> batchOperators = new ObservableCollection<BatchOperator>()
            {
                new BatchOperator("Jason", "Walker"),
                new BatchOperator("Geoff", "Nelson")
            };

            DataStore.BatchOperators = batchOperators;
        }

        private void AddOperator(object sender, RoutedEventArgs e)
        {
            DataStore.BatchOperators.Add(new BatchOperator(operatorFN.Text, operatorLN.Text));
        }
    }
}
