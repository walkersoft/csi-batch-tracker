using CSI.BatchTracker.ViewModels;
using System.Windows;

namespace CSI.BatchTracker.Views
{
    /// <summary>
    /// Interaction logic for ConnectedBatchInquiryWindow.xaml
    /// </summary>
    public partial class ConnectedBatchInquiryWindow : Window
    {
        public ConnectedBatchInquiryWindow(ImplementationInquiryViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
