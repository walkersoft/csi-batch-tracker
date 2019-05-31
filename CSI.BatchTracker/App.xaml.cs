using CSI.BatchTracker.Domain;
using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.DataSource.MemorySource;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.ViewModels;
using CSI.BatchTracker.Views;
using System.Windows;

namespace CSI.BatchTracker
{
    public partial class App : Application
    {
        MainWindow mainWindow;
        MainWindowViewModel mainWindowViewModel;
        IBatchOperatorSource operatorSource;
        IActiveInventorySource inventorySource;
        IReceivedBatchSource receivedBatchSource;
        IImplementedBatchSource implementedBatchSource;

        public void StartupBatchTRAX(object sender, StartupEventArgs e)
        {
            PrepareMainWindowViewModel();
            SetupMainWindowViewModelViewers();
            ShowMainWindow();
        }

        void PrepareMainWindowViewModel()
        {
            MemoryStoreContext context = new MemoryStoreContext();
            operatorSource = new MemoryBatchOperatorSource(context);
            inventorySource = new MemoryActiveInventorySource(context);
            receivedBatchSource = new MemoryReceivedBatchSource(context, inventorySource);
            implementedBatchSource = new MemoryImplementedBatchSource(context, inventorySource);

            mainWindowViewModel =  new MainWindowViewModel(inventorySource, receivedBatchSource, implementedBatchSource, operatorSource);
        }

        void SetupMainWindowViewModelViewers()
        {
            mainWindowViewModel.ReceivingManagementSessionViewer = new BatchReceivingManagementViewer(GetReceivingManagementViewModel());
            mainWindowViewModel.BatchOperatorManagementSessionViewer = new BatchOperatorManagementViewer(GetBatchOperatorViewModel());
            mainWindowViewModel.BatchHistoryViewer = new BatchHistoryViewer(GetBatchHistoryViewModel());
            mainWindowViewModel.ReceivingHistorySessionViewer = new ReceivingHistoryViewer(GetReceivingHistoryViewModel());
        }

        void ShowMainWindow()
        {
            mainWindow = new MainWindow(mainWindowViewModel);
            mainWindow.Show();
        }

        ReceivingManagementViewModel GetReceivingManagementViewModel()
        {
            return new ReceivingManagementViewModel(
                new DuracolorIntermixBatchNumberValidator(),
                new DuracolorIntermixColorList(),
                receivedBatchSource,
                operatorSource,
                inventorySource);
        }

        BatchOperatorViewModel GetBatchOperatorViewModel()
        {
            return new BatchOperatorViewModel(operatorSource);
        }

        BatchHistoryViewModel GetBatchHistoryViewModel()
        {
            return new BatchHistoryViewModel(
                new DuracolorIntermixBatchNumberValidator(),
                inventorySource,
                receivedBatchSource,
                implementedBatchSource);
        }

        ReceivingHistoryViewModel GetReceivingHistoryViewModel()
        {
            return new ReceivingHistoryViewModel(receivedBatchSource, inventorySource);
        }

        public void ShutdownBatchTRAX(object sender, ExitEventArgs e)
        {
            Current.Shutdown();
        }
    }
}
