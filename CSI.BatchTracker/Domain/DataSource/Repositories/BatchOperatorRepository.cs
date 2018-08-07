using CSI.BatchTracker.Contracts;
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
                Items.Add(entity);
            }
        }

        public List<Entity<BatchOperator>> FindById(int id)
        {
            List<Entity<BatchOperator>> results = new List<Entity<BatchOperator>>();

            if (store.BatchOperators.ContainsKey(id))
            {
                results.Add(store.BatchOperators[id]);
            }

            Items.Clear();
            Items.Add(store.BatchOperators[id]);

            return results;
        }
    }
}
