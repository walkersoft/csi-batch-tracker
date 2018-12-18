using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.ViewModels
{
    public sealed class BatchOperatorViewModel : ViewModelBase
    {
        public BatchOperator BatchOperator { get; }

        public BatchOperatorViewModel()
        {
            BatchOperator = new BatchOperator("Jason", "Walker");
        }

        public BatchOperatorViewModel(BatchOperator batchOperator)
        {
            BatchOperator = batchOperator;
        }

        public string FirstName
        {
            get { return BatchOperator.FirstName; }
            set
            {
                BatchOperator.FirstName = value;
                NotifyPropertyChanged("FirstName");
            }
        }

        public string LastName
        {
            get { return BatchOperator.LastName; }
            set
            {
                BatchOperator.LastName = value;
                NotifyPropertyChanged("LastName");
            }
        }
    }
}
