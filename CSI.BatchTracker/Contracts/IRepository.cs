using CSI.BatchTracker.Domain.DataSource;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Contracts
{
    public interface IRepository<T> where T : IEntity
    {
        ObservableCollection<T> Items { get; }

        void Save(T entity);
        List<T> FindById(int id);
        List<T> FindAll();
        List<T> FindAll(int limit);
        void Delete(int id);
    }
}
