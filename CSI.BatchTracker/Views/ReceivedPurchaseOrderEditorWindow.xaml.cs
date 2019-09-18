using CSI.BatchTracker.ViewModels;
using System.Windows;

namespace CSI.BatchTracker.Views
{
    /// <summary>
    /// Interaction logic for ReceivedPurchaseOrderEditorWindow.xaml
    /// </summary>
    public partial class ReceivedPurchaseOrderEditorWindow : Window
    {
        public ReceivedPurchaseOrderEditorWindow(ReceivingHistoryViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel.GetReceivedPurchaseOrderEditorViewModel();
        }
    }
}
