using CSI.BatchTracker.Storage.Contracts;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Storage.SQLiteStore;
using CSI.BatchTracker.ViewModels;
using Microsoft.Win32;
using System.Windows;

namespace CSI.BatchTracker.Views
{
    public partial class MainWindow : Window
    {
        IPersistenceManager<MemoryStoreContext> persistenceManager;
        MainWindowViewModel viewModel;

        public MainWindow(
            MainWindowViewModel viewModel, 
            IPersistenceManager<MemoryStoreContext> persistenceManager)
        {
            InitializeComponent();
            DataContext = viewModel;
            this.viewModel = viewModel;
            this.persistenceManager = persistenceManager;
        }

        private void SaveDataSource_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(persistenceManager.StoredContextLocation))
            {
                SaveFileDialog dialog = new SaveFileDialog
                {
                    Title = "Save Data Source As...",
                    Filter = "Data Source Files (*.dat)|*.dat"
                };

                dialog.ShowDialog();
                persistenceManager.StoredContextLocation = dialog.FileName;
            }

            persistenceManager.SaveDataSource();
        }

        private void LoadDataSource_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "Load Data Source",
                Filter = "Data Source Files (*.dat)|*.dat"
            };

            dialog.ShowDialog();
            persistenceManager.StoredContextLocation = dialog.FileName;
            persistenceManager.LoadDataSource();
            viewModel.AssociateCollectionsAndRepositories();
        }

        private void ClearDataSource_Click(object sender, RoutedEventArgs e)
        {
            persistenceManager.ClearDataSource();
            viewModel.AssociateCollectionsAndRepositories();
        }

        private void AttachDatabaseSource_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "Attach SQLite Database Source",
                Filter = "SQLite Database Files (*.sqlite3)|*.sqlite3"
            };

            dialog.ShowDialog();
            persistenceManager.StoredContextLocation = dialog.FileName;
            persistenceManager.LoadDataSource();
            viewModel.AssociateCollectionsAndRepositories();
        }
    }
}
