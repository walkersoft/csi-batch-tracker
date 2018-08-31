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
        public ObservableCollection<Entity<InventoryBatch>> Items => throw new NotImplementedException();
        DataStore store;

        public InventoryBatchRepository(DataStore store)
        {
            this.store = store;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Entity<InventoryBatch>> FindAll()
        {
            throw new NotImplementedException();
        }

        public List<Entity<InventoryBatch>> FindAll(int limit)
        {
            throw new NotImplementedException();
        }

        public List<Entity<InventoryBatch>> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public void Save(Entity<InventoryBatch> entity)
        {
            throw new NotImplementedException();
        }
    }
}
