using CSI.BatchTracker.ViewModels;
using MahApps.Metro.Controls;
using System.Windows;

namespace CSI.BatchTracker.Views
{
    /// <summary>
    /// Interaction logic for ReceivingHistoryWindow.xaml
    /// </summary>
    public partial class ReceivingHistoryWindow : MetroWindow
    {
        public ReceivingHistoryWindow(ReceivingHistoryViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
