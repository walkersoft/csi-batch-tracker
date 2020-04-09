using CSI.BatchTracker.ViewModels;
using MahApps.Metro.Controls;
using System.Windows;

namespace CSI.BatchTracker.Views
{
    /// <summary>
    /// Interaction logic for OperatorManagementWindowView.xaml
    /// </summary>
    public partial class BatchOperatorManagementWindow : MetroWindow
    {
        public BatchOperatorManagementWindow(BatchOperatorViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
