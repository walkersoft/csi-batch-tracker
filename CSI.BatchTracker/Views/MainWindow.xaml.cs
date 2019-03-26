using CSI.BatchTracker.ViewModels;
using System.Windows;

namespace CSI.BatchTracker.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel; 
        }
    }
}
