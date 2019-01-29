using CSI.BatchTracker.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
