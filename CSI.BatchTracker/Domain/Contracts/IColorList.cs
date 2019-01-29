using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Domain.Contracts
{
    public interface IColorList
    {
        int Count { get; }
        ObservableCollection<string> Colors { get; }
    }
}
