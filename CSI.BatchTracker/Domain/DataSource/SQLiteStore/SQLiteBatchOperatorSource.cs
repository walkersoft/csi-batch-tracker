using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Storage.Contracts;
using CSI.BatchTracker.Storage.SQLiteStore;
using CSI.BatchTracker.Storage.SQLiteStore.Transactions.BatchOperators;
using CSI.BatchTracker.Storage.SQLiteStore.Transactions.RecordAquisition;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CSI.BatchTracker.Domain.DataSource.SQLiteStore
{
    public sealed class SQLiteBatchOperatorSource : IBatchOperatorSource
    {
        SQLiteStoreContext sqliteStore;        
        public Dictionary<int, int> BatchOperatorIdMappings { get; private set; }

        public SQLiteBatchOperatorSource(SQLiteStoreContext sqliteStore)
        {
            this.sqliteStore = sqliteStore;
            OperatorRepository = new ObservableCollection<BatchOperator>();
            BatchOperatorIdMappings = new Dictionary<int, int>();
        }

        ObservableCollection<BatchOperator> operatorRepository;
        public ObservableCollection<BatchOperator> OperatorRepository
        {
            get
            {
                UpdateOperatorRepository();
                return operatorRepository;
            }
            private set
            {
                operatorRepository = value;
            }
        }

        void UpdateOperatorRepository()
        {
            ITransaction finder = new ListBatchOperatorsTransaction(sqliteStore);
            finder.Execute();

            operatorRepository.Clear();
            BatchOperatorIdMappings.Clear();

            for (int i = 0; i < finder.Results.Count; i++)
            {
                Entity<BatchOperator> entity = finder.Results[i] as Entity<BatchOperator>;
                operatorRepository.Add(entity.NativeModel);
                BatchOperatorIdMappings.Add(i, entity.SystemId);
            }
        }

        public void DeleteBatchOperator(int id)
        {
            ITransaction deleter = new DeleteBatchOperatorAtIdTransaction(id, sqliteStore);
            deleter.Execute();
            UpdateOperatorRepository();
        }

        public void FindAllBatchOperators()
        {
            UpdateOperatorRepository();
        }

        public BatchOperator FindBatchOperator(int id)
        {
            ITransaction finder = new FindBatchOperatorByIdTransaction(id, sqliteStore);
            finder.Execute();
            Entity<BatchOperator> entity = (Entity<BatchOperator>)finder.Results[0];

            return entity.NativeModel;
        }

        public void SaveOperator(BatchOperator batchOperator)
        {
            ITransaction adder = new AddBatchOperatorTransaction(new Entity<BatchOperator>(batchOperator), sqliteStore);
            adder.Execute();
            UpdateOperatorRepository();
        }

        public void UpdateOperator(int id, BatchOperator batchOperator)
        {
            ITransaction updater = new UpdateBatchOperatorTransaction(id, batchOperator, sqliteStore);
            updater.Execute();
            UpdateOperatorRepository();
        }

        public bool OperatorAtIdNotRelatedToOtherEntities(int id)
        {
            ITransaction finder = new CheckIfOperatorBelongsToRelatedEntitiesCommand(id, sqliteStore);
            finder.Execute();
            return finder.Results.Count == 0;
        }
    }
}
