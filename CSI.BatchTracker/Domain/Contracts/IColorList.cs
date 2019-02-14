using System.Collections.ObjectModel;

namespace CSI.BatchTracker.Domain.Contracts
{
    public interface IColorList
    {
        int Count { get; }
        ObservableCollection<string> Colors { get; }
    }
}
