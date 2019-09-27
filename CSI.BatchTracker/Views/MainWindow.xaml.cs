using CSI.BatchTracker.Storage.Contracts;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.ViewModels;
using System.Windows;

namespace CSI.BatchTracker.Views
{
    public partial class MainWindow : Window
    {
        IPersistenceManager<MemoryStoreContext> persistenceManager;
        MainWindowViewModel viewModel;

        public MainWindow(MainWindowViewModel viewModel, IPersistenceManager<MemoryStoreContext> persistenceManager)
        {
            InitializeComponent();
            DataContext = viewModel;
            this.viewModel = viewModel;
            this.persistenceManager = persistenceManager;
        }

        private void SaveDataSource_Click(object sender, RoutedEventArgs e)
        {
            persistenceManager.SaveDataSource();
        }

        private void LoadDataSource_Click(object sender, RoutedEventArgs e)
        {
            persistenceManager.LoadDataSource();
            viewModel.AssociateCollectionsAndRepositories();
        }

        private void ClearDataSource_Click(object sender, RoutedEventArgs e)
        {
            persistenceManager.ClearDataSource();
            viewModel.AssociateCollectionsAndRepositories();
        }
    }
}
