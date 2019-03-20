using CSI.BatchTracker.Domain;
using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.DataSource.MemorySource;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.ViewModels;
using CSI.BatchTracker.ViewModels.Commands;
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
            mainWindowViewModel = PrepareMainWindowViewModel();
            SetupMainWindowViewModelViewers();
            mainWindow = new MainWindow(mainWindowViewModel);
            mainWindow.Show();
        }

        MainWindowViewModel PrepareMainWindowViewModel()
        {
            MainWindowViewModel viewModel;
            MemoryStoreContext context = new MemoryStoreContext();
            operatorSource = new MemoryBatchOperatorSource(context);
            inventorySource = new MemoryActiveInventorySource(context);
            receivedBatchSource = new MemoryReceivedBatchSource(context, inventorySource);
            implementedBatchSource = new MemoryImplementedBatchSource(context, inventorySource);

            viewModel = new MainWindowViewModel(inventorySource, receivedBatchSource, implementedBatchSource, operatorSource);
            return viewModel;
        }

        void SetupMainWindowViewModelViewers()
        {
            mainWindowViewModel.ReceivingManagementSessionViewer = new BatchReceivingManagementViewer(GetReceivingManagementViewModel());
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

        public void ShutdownBatchTRAX(object sender, ExitEventArgs e)
        {
            Current.Shutdown();
        }
    }
}
