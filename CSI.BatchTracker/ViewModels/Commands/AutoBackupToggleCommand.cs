using CSI.BatchTracker.Properties;

namespace CSI.BatchTracker.ViewModels.Commands
{
    public sealed class AutoBackupToggleCommand : CommandBase
    {
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            Settings.Default.AutoDatabaseBackup = !Settings.Default.AutoDatabaseBackup;
            Settings.Default.Save();
        }
    }
}
