using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Storage.Contracts;
using CSI.BatchTracker.Storage.MemoryStore;
using CSI.BatchTracker.Storage.MemoryStore.Transactions.InventoryManagement;
using CSI.BatchTracker.Storage.MemoryStore.Transactions.RecordAquisition;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Domain.DataSource.MemorySource
{
    public class MemoryReceivedBatchSource : IReceivedBatchSource
    {
        public Dictionary<int, int> ReceivedBatchIdMappings { get; private set; }

        MemoryStoreContext memoryStore;

        public MemoryReceivedBatchSource(MemoryStoreContext memoryStore)
        {
            this.memoryStore = memoryStore;
            ReceivedBatchIdMappings = new Dictionary<int, int>();
            ReceivedBatchRepository = new ObservableCollection<ReceivedBatch>();
        }

        ObservableCollection<ReceivedBatch> receivedBatchRepository;
        public ObservableCollection<ReceivedBatch> ReceivedBatchRepository
        {
            get
            {
                return receivedBatchRepository;
            }
            private set
            {
                receivedBatchRepository = value;
            }
        }

        public void SaveReceivedBatch(ReceivedBatch receivedBatch)
        {
            Entity<ReceivedBatch> entity = new Entity<ReceivedBatch>(receivedBatch);
            ITransaction adder = new AddReceivedBatchToReceivingLedgerTransaction(entity, memoryStore);
            adder.Execute();
            UpdateReceivedBatchRepository();
        }

        void UpdateReceivedBatchRepository()
        {
            ITransaction finder = new ListReceivingLedgerTransaction(memoryStore);
            finder.Execute();

            receivedBatchRepository.Clear();
            ReceivedBatchIdMappings.Clear();
            int i = 0;

            foreach (Entity<ReceivedBatch> received in finder.Results)
            {
                receivedBatchRepository.Add(received.NativeModel);
                ReceivedBatchIdMappings.Add(i, received.SystemId);
                i++;
            }
        }

        public ReceivedBatch FindReceivedBatchById(int id)
        {
            ITransaction finder = new FindBatchInReceivingLedgerByIdTransaction(id, memoryStore);
            finder.Execute();
            Entity<ReceivedBatch> entity = (Entity<ReceivedBatch>)finder.Results[0];

            return entity.NativeModel;
        }

        public void UpdateReceivedBatch(int id, ReceivedBatch batch)
        {
            Entity<ReceivedBatch> entity = new Entity<ReceivedBatch>(id, batch);
            ITransaction updater = new EditBatchInReceivingLedgerTransaction(entity, memoryStore);
            updater.Execute();
        }

        public void DeleteReceivedBatch(int id)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<ReceivedBatch> FindReceivedBatchesByPONumber(int poNumber)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<ReceivedBatch> FindReceivedBatchesByDate(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
