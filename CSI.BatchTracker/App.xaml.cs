using CSI.BatchTracker.Domain;
using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.DataSource.SQLiteStore;
using CSI.BatchTracker.Properties;
using CSI.BatchTracker.Storage.SQLiteStore;
using CSI.BatchTracker.ViewModels;
using CSI.BatchTracker.Views;
using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
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

        public void StartupBatchTRAX(object sender, StartupEventArgs e)
        {
            EstablishDatabase();
            RunAutoBackupProcedure();
            PrepareMainWindowViewModel();
            SetupMainWindowViewModelViewers();
            ShowMainWindow();
        }

        void EstablishDatabase()
        {
            if (AttachedDatabaseIsSet())
            {
                if (FileIsSQLiteDatabase())
                {
                    return;
                }
                else
                {
                    CreateNewDatabaseFromDialogOrExit();
                }
            }

            CreateNewDatabaseFromDialogOrExit();
        }

        void CreateNewDatabaseFromDialogOrExit()
        {
            MessageBoxResult messageResult = MessageBox.Show("SQLite database not attached. Would you like to attach an existing database?", "Attach Database", MessageBoxButton.YesNo);

            if (messageResult == MessageBoxResult.Yes)
            {
                OpenFileDialog dialog = new OpenFileDialog
                {
                    Title = "Open SQLite Database",
                    Filter = "SQLite Database|*.sqlite3",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                };

                if (dialog.ShowDialog() == true)
                {
                    Settings.Default.AttachedDatabase = dialog.FileName;
                    Settings.Default.Save();
                }
            }

            if (messageResult == MessageBoxResult.No)
            {
                SaveFileDialog databaseDialog = new SaveFileDialog
                {
                    Title = "Create New SQLite Database",
                    Filter = "SQLite Database|*.sqlite3",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                };

                databaseDialog.ShowDialog();

                if (!string.IsNullOrEmpty(databaseDialog.FileName))
                {
                    SQLiteDatabaseInstaller installer = new SQLiteDatabaseInstaller
                    {
                        DatabaseFile = databaseDialog.FileName,
                        ConnectionString = string.Format("Data Source={0};Version=3;", databaseDialog.FileName)
                    };

                    installer.CreateNewDatabase();
                    Settings.Default.AttachedDatabase = databaseDialog.FileName;
                    Settings.Default.Save();

                    return;
                }

                MessageBox.Show("No database filename was given. A database is require for program operation.", "Failed to Create Database", MessageBoxButton.OK, MessageBoxImage.Error);
                Current.Shutdown(1);
                Environment.Exit(1);
            }
        }

        void RunAutoBackupProcedure()
        {
            if (Settings.Default.AutoDatabaseBackup && AttachedDatabaseIsSet())
            {
                DateTime date = DateTime.Now;
                string filename = string.Format(@"\SQLiteBackup_{0}.sqlite3", date.ToString("yyyy-MM-dd_HH-mm-ss"));
                string path = Path.GetDirectoryName(Settings.Default.AttachedDatabase);
                File.Copy(Settings.Default.AttachedDatabase, path + filename, true);
            }
        }

        bool FileIsSQLiteDatabase()
        {
            if (File.Exists(Settings.Default.AttachedDatabase))
            {
                byte[] bytes = new byte[17];

                using (FileStream stream = new FileStream(Settings.Default.AttachedDatabase, FileMode.Open, FileAccess.Read))
                {
                    stream.Read(bytes, 0, 16);
                    return ASCIIEncoding.ASCII.GetString(bytes).Contains("SQLite format");
                }
            }

            return false;
        }

        bool AttachedDatabaseIsSet()
        {
            return string.IsNullOrEmpty(Settings.Default.AttachedDatabase) == false;
        }

        void PrepareMainWindowViewModel()
        {
            SQLiteStoreContext sqliteStore = new SQLiteStoreContext(Settings.Default.AttachedDatabase);
            operatorSource = new SQLiteBatchOperatorSource(sqliteStore);
            inventorySource = new SQLiteActiveInventorySource(sqliteStore);
            receivedBatchSource = new SQLiteReceivedBatchSource(sqliteStore, inventorySource);
            implementedBatchSource = new SQLiteImplementedBatchSource(sqliteStore, inventorySource);

            mainWindowViewModel =  new MainWindowViewModel(inventorySource, receivedBatchSource, implementedBatchSource, operatorSource);
        }

        void SetupMainWindowViewModelViewers()
        {
            ReceivingManagementViewModel receivingManagementViewModel = GetReceivingManagementViewModel();
            mainWindowViewModel.ReceivingManagementSessionViewer = new BatchReceivingManagementViewer(receivingManagementViewModel);
            mainWindowViewModel.BatchOperatorManagementSessionViewer = new BatchOperatorManagementViewer(GetBatchOperatorViewModel());
            mainWindowViewModel.BatchHistoryViewer = new BatchHistoryViewer(GetBatchHistoryViewModel());
            mainWindowViewModel.ReceivingHistorySessionViewer = new ReceivingHistoryViewer(GetReceivingHistoryViewModel(receivingManagementViewModel));
            mainWindowViewModel.ConnectedBatchInquiryViewer = new ConnectedBatchInquiryViewer(GetImplementationInquiryViewModel());
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

        ImplementationInquiryViewModel GetImplementationInquiryViewModel()
        {
            return new ImplementationInquiryViewModel(implementedBatchSource);
        }

        public void ShutdownBatchTRAX(object sender, ExitEventArgs e)
        {
            Current.Shutdown();
        }
    }
}
