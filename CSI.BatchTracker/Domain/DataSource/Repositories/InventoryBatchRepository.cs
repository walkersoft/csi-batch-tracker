using CSI.BatchTracker.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Experimental;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Domain.DataSource.Repositories
{
    public class InventoryBatchRepository : IRepository<Entity<InventoryBatch>>
    {
        public ObservableCollection<Entity<InventoryBatch>> Items { get; private set; }
        DataStore store;

        public InventoryBatchRepository(DataStore store)
        {
            this.store = store;
            Items = new ObservableCollection<Entity<InventoryBatch>>();
        }

        public void Delete(int id)
        {
            if (store.InventoryBatches.ContainsKey(id))
            {
                store.InventoryBatches.Remove(id);
            }
        }

        public List<Entity<InventoryBatch>> FindAll()
        {
            List<Entity<InventoryBatch>> batches = new List<Entity<InventoryBatch>>();

            for (int i = 1; i <= store.InventoryBatches.Count; i++)
            {
                batches.Add(store.InventoryBatches[i]);
            }

            return batches;
        }

        public List<Entity<InventoryBatch>> FindAll(int limit)
        {
            throw new NotImplementedException();
        }

        public List<Entity<InventoryBatch>> FindById(int id)
        {
            List<Entity<InventoryBatch>> results = new List<Entity<InventoryBatch>>();

            if (store.InventoryBatches.ContainsKey(id))
            {
                results.Add(store.InventoryBatches[id]);
            }

            return results;
        }

        public void Save(Entity<InventoryBatch> entity)
        {
            if (entity.SystemId == 0)
            {
                entity = new Entity<InventoryBatch>(store.InventoryBatches.Count + 1, entity.NativeModel);
            }

            store.InventoryBatches.Add(entity.SystemId, entity);
        }
    }
}
