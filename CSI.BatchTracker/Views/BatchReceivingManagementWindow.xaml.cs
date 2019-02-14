using CSI.BatchTracker.ViewModels;
using System.Windows;

namespace CSI.BatchTracker.Views
{
    /// <summary>
    /// Interaction logic for BatchReceivingManagementWindow.xaml
    /// </summary>
    public partial class BatchReceivingManagementWindow : Window
    {
        public BatchReceivingManagementWindow(ReceivingManagementViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
