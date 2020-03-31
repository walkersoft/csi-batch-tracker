using CSI.BatchTracker.Domain.Contracts;
using System.Collections.ObjectModel;

namespace CSI.BatchTracker.Domain
{
    public sealed class DuracolorIntermixColorList : IColorList
    {
        public int Count { get { return Colors.Count; } }
        public ObservableCollection<string> Colors
        {
            get
            {
                return new ObservableCollection<string>
                {
                    "White", "Black", "Yellow", "Red", "Blue Red", "Deep Blue", "Deep Green", "Bright Red", "Bright Yellow"
                };
            }
        }
    }
}
