using CSI.BatchTracker.ViewModels;
using MahApps.Metro.Controls;

namespace CSI.BatchTracker.Views
{
    /// <summary>
    /// Interaction logic for BatchReceivingManagementWindow.xaml
    /// </summary>
    public partial class BatchReceivingManagementWindow : MetroWindow
    {
        public BatchReceivingManagementWindow(ReceivingManagementViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
