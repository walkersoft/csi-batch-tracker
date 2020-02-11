using CSI.BatchTracker.Domain;
using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.DataSource.SQLiteStore;
using CSI.BatchTracker.Storage.Contracts;
using CSI.BatchTracker.Storage.SQLiteStore;
using CSI.BatchTracker.ViewModels;
using CSI.BatchTracker.Views;
using System.Windows;

namespace CSI.BatchTracker
{
    public partial class App : Application
    {
        MainWindow mainWindow;
        MainWindowViewModel mainWindowViewModel;
        IBatchOperatorSource operatorSource;
        IActiveInventorySource inventorySource;
        IReceivedBatchSource receivedBatchSource;
        IImplementedBatchSource implementedBatchSource;
        IPersistenceManager<SQLiteStoreContext> dbManager;

        public void StartupBatchTRAX(object sender, StartupEventArgs e)
        {
            //SetupMemoryStorePeristenceManager();
            PrepareMainWindowViewModel();
            SetupMainWindowViewModelViewers();
            ShowMainWindow();
        }

        void PrepareMainWindowViewModel()
        {
            /*
            operatorSource = new MemoryBatchOperatorSource(memoryStorePersistence.Context);
            inventorySource = new MemoryActiveInventorySource(memoryStorePersistence.Context);
            receivedBatchSource = new MemoryReceivedBatchSource(memoryStorePersistence.Context, inventorySource);
            implementedBatchSource = new MemoryImplementedBatchSource(memoryStorePersistence.Context, inventorySource);
            */
            SQLiteStoreContext sqliteStore = new SQLiteStoreContext("C:\\Users\\jwalker\\Documents\\BatchTRAX\\test_sqlitestore.sqlite3");
            operatorSource = new SQLiteBatchOperatorSource(sqliteStore);
            inventorySource = new SQLiteActiveInventorySource(sqliteStore);
            receivedBatchSource = new SQLiteReceivedBatchSource(sqliteStore, inventorySource);
            implementedBatchSource = new SQLiteImplementedBatchSource(sqliteStore, inventorySource);

            mainWindowViewModel =  new MainWindowViewModel(inventorySource, receivedBatchSource, implementedBatchSource, operatorSource);
        }

        public void AttachSQLiteDatabase(string contextLocation)
        {

        }

        void SetupMemoryStorePeristenceManager()
        {
            //string fileLocation = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\BatchTRAX_MemoryStore.dat";
            //memoryStorePersistence = new MemoryStorePersistenceManager();
        }

        void SetupMainWindowViewModelViewers()
        {
            ReceivingManagementViewModel receivingManagementViewModel = GetReceivingManagementViewModel();
            mainWindowViewModel.ReceivingManagementSessionViewer = new BatchReceivingManagementViewer(receivingManagementViewModel);
            mainWindowViewModel.BatchOperatorManagementSessionViewer = new BatchOperatorManagementViewer(GetBatchOperatorViewModel());
            mainWindowViewModel.BatchHistoryViewer = new BatchHistoryViewer(GetBatchHistoryViewModel());
            mainWindowViewModel.ReceivingHistorySessionViewer = new ReceivingHistoryViewer(GetReceivingHistoryViewModel(receivingManagementViewModel));
        }

        void ShowMainWindow()
        {
            mainWindow = new MainWindow(mainWindowViewModel);
            mainWindow.Show();
        }

        ReceivingManagementViewModel GetReceivingManagementViewModel()
        {
            return new ReceivingManagementViewModel(
                new DuracolorIntermixBatchNumberValidator(),
                new DuracolorIntermixColorList(),
                receivedBatchSource,
                operatorSource,
                inventorySource);
        }

        BatchOperatorViewModel GetBatchOperatorViewModel()
        {
            return new BatchOperatorViewModel(operatorSource);
        }

        BatchHistoryViewModel GetBatchHistoryViewModel()
        {
            return new BatchHistoryViewModel(
                new DuracolorIntermixBatchNumberValidator(),
                inventorySource,
                receivedBatchSource,
                implementedBatchSource);
        }

        ReceivingHistoryViewModel GetReceivingHistoryViewModel(ReceivingManagementViewModel receivingManagementViewModel)
        {
            return new ReceivingHistoryViewModel(receivedBatchSource, inventorySource, operatorSource, implementedBatchSource, receivingManagementViewModel);
        }

        public void ShutdownBatchTRAX(object sender, ExitEventArgs e)
        {
            Current.Shutdown();
        }
    }
}
