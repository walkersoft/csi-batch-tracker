using CSI.BatchTracker.ViewModels;
using System.Windows;

namespace CSI.BatchTracker.Views
{
    public partial class BatchHistoryWindow : Window
    {
        public BatchHistoryWindow(BatchHistoryViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
