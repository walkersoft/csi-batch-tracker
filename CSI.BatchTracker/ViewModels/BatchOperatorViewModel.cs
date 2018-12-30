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
        int batchOperatorComboBoxIndex;
        int batchOperatorListBoxIndex;

        public BatchOperatorViewModel(IDataSource dataSource)
        {
            BatchOperator = new BatchOperator("", "");
            InstantiateCommands();
            validator = new BatchOperatorValidator();
            DataSource = dataSource;
            SelectedBatchOperatorFromComboBoxIndex = -1;
            OperatorRepository = DataSource.OperatorRepository;
        }

        void InstantiateCommands()
        {
            SaveBatchOperator = new SaveBatchOperatorCommand(this);
            BatchOperatorComboBoxChanged = new BatchOperatorComboBoxChangedCommand(this);
            BatchOperatorListBoxChanged = new BatchOperatorListBoxChangedCommand(this);
        }

        public ObservableCollection<string> OperatorNames
        {
            get
            {
                GenerateOperatorSelectionItemsSource();
                return operatorNames;
            }
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
            SelectedBatchOperatorFromComboBoxIndex = -1;
        }

        void UpdateBatchOperator(BatchOperator batchOperator)
        {
            BatchOperator = batchOperator;
            NotifyPropertyChanged("FirstName");
            NotifyPropertyChanged("LastName");
        }

        public int SelectedBatchOperatorFromComboBoxIndex
        {
            get { return batchOperatorComboBoxIndex; }
            set
            {
                batchOperatorComboBoxIndex = value;
                NotifyPropertyChanged("SelectedBatchOperatorFromComboBoxIndex");
            }
        }

        public int SelectedBatchOperatorFromListBoxIndex
        {
            get { return batchOperatorListBoxIndex; }
            set
            {
                batchOperatorListBoxIndex = value;
                NotifyPropertyChanged("SelectedBatchOperatorFromListBoxIndex");
            }
        }

        public void PopulateBatchOperatorOrCreateNew()
        {
            if (SelectedBatchOperatorFromComboBoxIndex == 0)
            {
                ResetBatchOperator();
            }
            else
            {
                int targetId = DataSource.BatchOperatorIdMappings[SelectedBatchOperatorFromComboBoxIndex - 1];
                UpdateBatchOperator(DataSource.FindBatchOperatorById(targetId));
            }
        }

        public void MatchComboBoxOperatorWithListBoxOperator()
        {
            SelectedBatchOperatorFromComboBoxIndex = SelectedBatchOperatorFromListBoxIndex + 1;
            PopulateBatchOperatorOrCreateNew();
        }
    }
}
