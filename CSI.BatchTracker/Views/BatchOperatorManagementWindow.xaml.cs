using CSI.BatchTracker.ViewModels;
using System.Windows;

namespace CSI.BatchTracker.Views
{
    /// <summary>
    /// Interaction logic for OperatorManagementWindowView.xaml
    /// </summary>
    public partial class BatchOperatorManagementWindow : Window
    {
        public BatchOperatorManagementWindow(BatchOperatorViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
