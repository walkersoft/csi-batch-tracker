using CSI.BatchTracker.Commands;
using CSI.BatchTracker.Contracts;
using CSI.BatchTracker.Domain;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CSI.BatchTracker.ViewModels
{
    public sealed class BatchOperatorViewModel : ViewModelBase
    {
        public BatchOperator BatchOperator { get; set; }
        public IDataSource DataSource { get; private set; }
        BatchOperatorValidator validator;

        public BatchOperatorViewModel(IDataSource dataSource)
        {
            BatchOperator = new BatchOperator("", "");
            SaveBatchOperator = new SaveBatchOperatorCommand(this);
            validator = new BatchOperatorValidator();
            DataSource = dataSource;
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

        public ICommand SaveBatchOperator { get; private set; }

        public bool BatchOperatorIsValid()
        {
            return validator.Validate(BatchOperator);
        }

        public void PersistBatchOperator()
        {
            BatchOperator batchOperator = new BatchOperator(BatchOperator.FirstName, BatchOperator.LastName);
            DataSource.SaveOperator(batchOperator);
            ResetBatchOperator();
        }

        void ResetBatchOperator()
        {            
            FirstName = "";
            LastName = "";
        }
    }
}
