using CSI.BatchTracker.ViewModels;
using CSI.BatchTracker.ViewModels.Commands;

namespace CSI.BatchTracker.DemoTools
{
    internal class LaunchDataSourcePopulatorCommand : CommandBase
    {
        MainWindowViewModel viewModel;
        bool needsToRun = true;

        public LaunchDataSourcePopulatorCommand(MainWindowViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return needsToRun;
        }

        public override void Execute(object parameter)
        {
            viewModel.RunPopulatorTool();
            needsToRun = !needsToRun;
        }
    }
}
