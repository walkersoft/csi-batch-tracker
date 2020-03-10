using CSI.BatchTracker.Storage.Contracts;
using CSI.BatchTracker.Storage.SQLiteStore;
using CSI.BatchTracker.ViewModels;
using Microsoft.Win32;
using System;
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
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Open SQLite Database";
            dialog.Filter = "SQLite Database|*.sqlite3";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

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
    }
}
