using CSI.BatchTracker.ViewModels;
using MahApps.Metro.Controls;

namespace CSI.BatchTracker.Views
{
    /// <summary>
    /// Interaction logic for ReceivedPurchaseOrderEditorWindow.xaml
    /// </summary>
    public partial class ReceivedPurchaseOrderEditorWindow : MetroWindow
    {
        public ReceivedPurchaseOrderEditorWindow(ReceivingHistoryViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel.GetReceivedPurchaseOrderEditorViewModel();
        }
    }
}
