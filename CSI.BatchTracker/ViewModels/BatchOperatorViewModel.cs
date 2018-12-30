using CSI.BatchTracker.Commands;
using CSI.BatchTracker.Contracts;
using CSI.BatchTracker.Domain;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CSI.BatchTracker.ViewModels
{
    public sealed class BatchOperatorViewModel : ViewModelBase
    {
        public ICommand SaveBatchOperator { get; private set; }
        public ICommand BatchOperatorComboBoxChanged { get; private set; }
        public IDataSource DataSource { get; private set; }

        public BatchOperator BatchOperator { get; set; }
        public int UserSelectedComboBoxIndex { get; set; }
        public ObservableCollection<BatchOperator> OperatorRepository { get; private set; }

        public ObservableCollection<string> OperatorNames
        {
            get
            {
                GenerateOperatorSelectionItemsSource();
                return operatorNames;
            }
        }

        BatchOperatorValidator validator;
        ObservableCollection<string> operatorNames;

        public BatchOperatorViewModel(IDataSource dataSource)
        {
            BatchOperator = new BatchOperator("", "");
            SaveBatchOperator = new SaveBatchOperatorCommand(this);
            BatchOperatorComboBoxChanged = new BatchOperatorComboBoxChangedCommand(this);
            validator = new BatchOperatorValidator();
            DataSource = dataSource;
            UserSelectedComboBoxIndex = -1;
            OperatorRepository = DataSource.OperatorRepository;
        }

        public void GenerateOperatorSelectionItemsSource()
        {
            operatorNames = new ObservableCollection<string>
            {
                "<Create New...>"
            };

            foreach (BatchOperator batchOperator in DataSource.OperatorRepository)
            {
                operatorNames.Add(batchOperator.FullName);
            }
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

        public bool BatchOperatorIsValid()
        {
            return validator.Validate(BatchOperator);
        }

        public void PersistBatchOperator()
        {
            BatchOperator batchOperator = new BatchOperator(BatchOperator.FirstName, BatchOperator.LastName);
            DataSource.SaveOperator(batchOperator);
            ResetBatchOperator();
            NotifyPropertyChanged("OperatorNames");
        }

        void ResetBatchOperator()
        {
            UpdateBatchOperator(new BatchOperator("", ""));
        }

        void UpdateBatchOperator(BatchOperator batchOperator)
        {
            BatchOperator = batchOperator;
            NotifyPropertyChanged("FirstName");
            NotifyPropertyChanged("LastName");
        }

        public void PopulateBatchOperatorOrCreateNew()
        {
            if (UserSelectedComboBoxIndex == 0)
            {
                ResetBatchOperator();
            }
            else
            {
                int targetId = DataSource.BatchOperatorIdMappings[UserSelectedComboBoxIndex - 1];
                UpdateBatchOperator(DataSource.FindBatchOperatorById(targetId));
            }
        }
    }
}
