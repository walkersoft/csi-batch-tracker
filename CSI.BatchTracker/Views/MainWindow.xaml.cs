using CSI.BatchTracker.Storage.Contracts;
using CSI.BatchTracker.Storage.SQLiteStore;
using CSI.BatchTracker.ViewModels;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

namespace CSI.BatchTracker.Views
{
    public partial class MainWindow : Window
    {
        MainWindowViewModel viewModel;

        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            this.viewModel = viewModel;
        }

        private void AttachDatabase(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "Open SQLite Database",
                Filter = "SQLite Database|*.sqlite3",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            if (dialog.ShowDialog() == true)
            {
                Properties.Settings.Default.AttachedDatabase = dialog.FileName;
                Properties.Settings.Default.Save();

                MessageBox.Show(
                    "New SQLite data source file saved. You must restart the application for settings to take effect.",
                    "Data Source Saved"
                );
            }
        }

        private void CreateDatabaseBackup(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Title = "Backup BatchTRAX Database",
                Filter = "SQLite Database|*.sqlite3",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            if (dialog.ShowDialog() == true)
            {
                if (string.IsNullOrEmpty(dialog.FileName))
                {
                    MessageBox.Show("No filename given. Backup operation canceled.", "Backup Failed");
                    return;
                }

                string currentFile = Properties.Settings.Default.AttachedDatabase;
                File.Copy(currentFile, dialog.FileName, true);
            }
        }
    }
}
