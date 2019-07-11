using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Domain
{
    public static class CollectionConversionExtentions
    {
        public static List<ReceivedBatch> ToList(this ObservableCollection<ReceivedBatch> original)
        {
            return new List<ReceivedBatch>(original as IEnumerable<ReceivedBatch>);
        }
    }
}
