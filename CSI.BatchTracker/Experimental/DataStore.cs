using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Experimental
{
    public class DataStore
    {
        public ObservableCollection<BatchOperator> BatchOperators { get; set; }

        public DataStore()
        {

        }
    }
}
