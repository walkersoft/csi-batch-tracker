using CSI.BatchTracker.Domain.Contracts;
using System.Collections.ObjectModel;

namespace CSI.BatchTracker.Domain
{
    public class DuracolorIntermixColorList : IColorList
    {
        public int Count { get { return Colors.Count; } }

        ObservableCollection<string> colors;
        public ObservableCollection<string> Colors
        {
            get
            {
                return colors ?? new ObservableCollection<string>
                {
                    "White", "Black", "Yellow", "Red", "Blue Red", "Deep Blue", "Deep Green", "Bright Red", "Bright Yellow"
                };
            }
            private set
            {
                colors = value;
            }
        }
    }
}
