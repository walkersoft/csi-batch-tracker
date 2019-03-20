using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Storage.Contracts;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Storage.MemoryStore.Transactions.BatchOperators;
using CSI.BatchTracker.Storage.MemoryStore.Transactions.RecordAquisition;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CSI.BatchTracker.Domain.DataSource.MemorySource
{
    public class MemoryBatchOperatorSource : IBatchOperatorSource
    {
        public Dictionary<int, int> BatchOperatorIdMappings { get; private set; }

        MemoryStoreContext memoryStore;

        public MemoryBatchOperatorSource(MemoryStoreContext memoryStore)
        {
            this.memoryStore = memoryStore;
            BatchOperatorIdMappings = new Dictionary<int, int>();
            operatorRepository = new ObservableCollection<BatchOperator>();
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

        public void SaveOperator(BatchOperator batchOperator)
        {
            ITransaction adder = new AddBatchOperatorTransaction(new Entity<BatchOperator>(batchOperator), memoryStore);
            adder.Execute();
            UpdateOperatorRepository();
        }

        public void UpdateOperator(int id, BatchOperator batchOperator)
        {
            Entity<BatchOperator> entity = new Entity<BatchOperator>(id, batchOperator);
            ITransaction updater = new UpdateBatchOperatorTransaction(entity, memoryStore);
            updater.Execute();
            UpdateOperatorRepository();
        }

        public BatchOperator FindBatchOperator(int id)
        {
            ITransaction finder = new FindBatchOperatorByIdTransaction(id, memoryStore);
            finder.Execute();
            Entity<BatchOperator> entity = (Entity<BatchOperator>)finder.Results[0];

            return entity.NativeModel;
        }

        public void DeleteBatchOperator(int id)
        {
            ITransaction remover = new DeleteBatchOperatorAtIdTransaction(id, memoryStore);
            remover.Execute();
            UpdateOperatorRepository();
        }

        void UpdateOperatorRepository()
        {
            ITransaction finder = new ListBatchOperatorsTransaction(memoryStore);
            finder.Execute();

            operatorRepository.Clear();
            BatchOperatorIdMappings.Clear();
            int i = 0;

            foreach (Entity<BatchOperator> batchOperator in finder.Results)
            {
                BatchOperatorIdMappings.Add(i, batchOperator.SystemId);
                operatorRepository.Add(batchOperator.NativeModel);
                i++;
            }
        }

        public void FindAllBatchOperators()
        {
            UpdateOperatorRepository();
        }
    }
}
