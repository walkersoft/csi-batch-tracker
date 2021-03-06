﻿using CSI.BatchTracker.Domain;
using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.ViewModels.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CSI.BatchTracker.ViewModels
{
    public sealed class BatchOperatorViewModel : ViewModelBase
    {
        BatchOperatorValidator validator;
        IBatchOperatorSource operatorSource;
        public ICommand SaveBatchOperator { get; private set; }
        public ICommand DeleteSelectedBatchOperator { get; private set; }
        public ICommand BatchOperatorComboBoxChanged { get; private set; }
        public ICommand BatchOperatorListBoxChanged { get; private set; }
        public BatchOperator BatchOperator { get; set; }
        public ObservableCollection<BatchOperator> OperatorRepository { get; private set; }

        public BatchOperatorViewModel(IBatchOperatorSource operatorSource)
        {
            this.operatorSource = operatorSource;
            OperatorRepository = operatorSource.OperatorRepository;
            SelectedBatchOperatorFromListBoxIndex = -1;
            validator = new BatchOperatorValidator();
            BatchOperator = new BatchOperator("", "");
            SaveBatchOperator = new SaveBatchOperatorCommand(this);
            DeleteSelectedBatchOperator = new DeleteSelectedBatchOperatorCommand(this);
            BatchOperatorComboBoxChanged = new BatchOperatorComboBoxChangedCommand(this);
            BatchOperatorListBoxChanged = new BatchOperatorListBoxChangedCommand(this);
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

        ObservableCollection<string> operatorNames;
        public ObservableCollection<string> OperatorNames
        {
            get
            {
                GenerateOperatorSelectionItemsSource();
                return operatorNames;
            }
        }

        void GenerateOperatorSelectionItemsSource()
        {
            operatorNames = new ObservableCollection<string>
            {
                "<Create New...>"
            };

            foreach (BatchOperator batchOperator in operatorSource.OperatorRepository)
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
            BatchOperator batchOperator = new BatchOperator(FirstName, LastName);

            if (SelectedBatchOperatorFromComboBoxIndex > 0)
            {
                int targetId = operatorSource.BatchOperatorIdMappings[SelectedBatchOperatorFromComboBoxIndex - 1];
                operatorSource.UpdateOperator(targetId, batchOperator);
            }
            else
            {
                operatorSource.SaveOperator(batchOperator);
            }

            ResetBatchOperator();
            SelectedBatchOperatorFromComboBoxIndex = -1;
            NotifyPropertyChanged("OperatorNames");
        }

        void ResetBatchOperator()
        {
            UpdateActiveBatchOperator(new BatchOperator("", ""));
        }

        void UpdateActiveBatchOperator(BatchOperator batchOperator)
        {
            FirstName = batchOperator.FirstName;
            LastName = batchOperator.LastName;
        }

        public void PopulateBatchOperatorOrReset()
        {
            if (SelectedBatchOperatorFromComboBoxIndex > 0)
            {
                int targetId = operatorSource.BatchOperatorIdMappings[SelectedBatchOperatorFromComboBoxIndex - 1];
                UpdateActiveBatchOperator(operatorSource.FindBatchOperator(targetId));
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

        public bool BatchOperatorIsRemoveable()
        {
            if (SelectedBatchOperatorFromListBoxIndex > -1)
            {
                return operatorSource.OperatorAtIdNotRelatedToOtherEntities(operatorSource.BatchOperatorIdMappings[SelectedBatchOperatorFromListBoxIndex]);
            }

            return false;
        }

        public void RemoveSelectedBatchOperator()
        {
            int targetId = operatorSource.BatchOperatorIdMappings[SelectedBatchOperatorFromListBoxIndex];
            operatorSource.DeleteBatchOperator(targetId);
            ResetBatchOperator();
            SelectedBatchOperatorFromComboBoxIndex = -1;
            NotifyPropertyChanged("OperatorNames");
            NotifyPropertyChanged("OperatorRepository");
        }
    }
}
