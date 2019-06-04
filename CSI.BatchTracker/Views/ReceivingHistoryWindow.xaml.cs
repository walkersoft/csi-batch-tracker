using CSI.BatchTracker.ViewModels;
using System.Windows;

namespace CSI.BatchTracker.Views
{
    /// <summary>
    /// Interaction logic for ReceivingHistoryWindow.xaml
    /// </summary>
    public partial class ReceivingHistoryWindow : Window
    {
        public ReceivingHistoryWindow(ReceivingHistoryViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
