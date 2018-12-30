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
        public ICommand BatchOperatorListBoxChanged { get; private set; }

        public IDataSource DataSource { get; private set; }

        public BatchOperator BatchOperator { get; set; }
        public ObservableCollection<BatchOperator> OperatorRepository { get; private set; }

        BatchOperatorValidator validator;

        ObservableCollection<string> operatorNames;
        public ObservableCollection<string> OperatorNames
        {
            get
            {
                GenerateOperatorSelectionItemsSource();
                return operatorNames;
            }
        }

        int batchOperatorComboBoxIndex;
        public int SelectedBatchOperatorFromComboBoxIndex
        {
            get { return batchOperatorComboBoxIndex; }
            set
            {
                batchOperatorComboBoxIndex = value;
                NotifyPropertyChanged("SelectedBatchOperatorFromComboBoxIndex");
            }
        }

        int batchOperatorListBoxIndex;
        public int SelectedBatchOperatorFromListBoxIndex
        {
            get { return batchOperatorListBoxIndex; }
            set
            {
                batchOperatorListBoxIndex = value;
                NotifyPropertyChanged("SelectedBatchOperatorFromListBoxIndex");
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

        public BatchOperatorViewModel(IDataSource dataSource)
        {
            DataSource = dataSource;
            OperatorRepository = DataSource.OperatorRepository;

            UpdateActiveBatchOperator(new BatchOperator("", ""));
            validator = new BatchOperatorValidator();
            SelectedBatchOperatorFromComboBoxIndex = -1;

            SaveBatchOperator = new SaveBatchOperatorCommand(this);
            BatchOperatorComboBoxChanged = new BatchOperatorComboBoxChangedCommand(this);
            BatchOperatorListBoxChanged = new BatchOperatorListBoxChangedCommand(this);
        }

        void UpdateActiveBatchOperator(BatchOperator batchOperator)
        {
            BatchOperator.FirstName = batchOperator.FirstName;
            BatchOperator.LastName = batchOperator.LastName;
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
            UpdateActiveBatchOperator(new BatchOperator("", ""));
            SelectedBatchOperatorFromComboBoxIndex = -1;
        }

        public void PopulateBatchOperatorOrReset()
        {
            if (SelectedBatchOperatorFromComboBoxIndex > 0)
            {
                int targetId = DataSource.BatchOperatorIdMappings[SelectedBatchOperatorFromComboBoxIndex - 1];
                UpdateActiveBatchOperator(DataSource.FindBatchOperatorById(targetId));
            }
            else
            { 
                ResetBatchOperator();
            }
        }

        public void MatchComboBoxOperatorWithListBoxOperator()
        {
            SelectedBatchOperatorFromComboBoxIndex = SelectedBatchOperatorFromListBoxIndex + 1;
            PopulateBatchOperatorOrReset();
        }
    }
}
