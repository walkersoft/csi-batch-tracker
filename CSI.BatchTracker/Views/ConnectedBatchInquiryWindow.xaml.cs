using CSI.BatchTracker.ViewModels;
using MahApps.Metro.Controls;
using System.Windows;

namespace CSI.BatchTracker.Views
{
    /// <summary>
    /// Interaction logic for ConnectedBatchInquiryWindow.xaml
    /// </summary>
    public partial class ConnectedBatchInquiryWindow : MetroWindow
    {
        public ConnectedBatchInquiryWindow(ImplementationInquiryViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
