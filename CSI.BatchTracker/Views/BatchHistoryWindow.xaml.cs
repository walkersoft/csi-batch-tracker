using CSI.BatchTracker.ViewModels;
using MahApps.Metro.Controls;
using System.Windows;

namespace CSI.BatchTracker.Views
{
    public partial class BatchHistoryWindow : MetroWindow
    {
        public BatchHistoryWindow(BatchHistoryViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
