using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace CSI.BatchTracker.ViewModels
{
    [ExcludeFromCodeCoverage]
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
