using CSI.BatchTracker.Storage.Contracts;
using CSI.BatchTracker.Storage.SQLiteStore;
using CSI.BatchTracker.ViewModels;
using Microsoft.Win32;
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
    }
}
