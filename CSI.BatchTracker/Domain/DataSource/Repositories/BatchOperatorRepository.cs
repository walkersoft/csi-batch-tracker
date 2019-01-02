using CSI.BatchTracker.Contracts;
using CSI.BatchTracker.Storage.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Experimental;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Domain.DataSource.Repositores
{
    public class BatchOperatorRepository : IRepository<Entity<BatchOperator>>
    {
        public ObservableCollection<Entity<BatchOperator>> Items { get; private set; }
        DataStore store;

        public BatchOperatorRepository(DataStore store)
        {
            Items = new ObservableCollection<Entity<BatchOperator>>();
            this.store = store;
        }

        public void Save(Entity<BatchOperator> entity)
        {
            if (entity.SystemId == 0)
            {
                int index = store.BatchOperators.Count + 1;
                store.BatchOperators.Add(index, entity);
            }
            else
            {
                store.BatchOperators.Add(entity.SystemId, entity);
            }

            Items.Add(entity);
        }

        public List<Entity<BatchOperator>> FindById(int id)
        {
            List<Entity<BatchOperator>> results = new List<Entity<BatchOperator>>();

            if (store.BatchOperators.ContainsKey(id))
            {
                results.Add(store.BatchOperators[id]);
                Items.Clear();
                Items.Add(store.BatchOperators[id]);
            }

            return results;
        }

        public List<Entity<BatchOperator>> FindAll()
        {
            return store.BatchOperators.Values.ToList();
        }

        public List<Entity<BatchOperator>> FindAll(int limit)
        {
            List<Entity<BatchOperator>> results = new List<Entity<BatchOperator>>();
            List<Entity<BatchOperator>> records = FindAll();
            
            for (int i = 0; i < limit; ++i)
            {
                results.Add(records[i]);
            }

            return results;
        }

        public void Delete(int id)
        {
            store.BatchOperators.Remove(id);
        }
    }
}
