using CSI.BatchTracker.Domain.DataSource;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Contracts
{
    public interface IRepository<T>
    {
        ObservableCollection<T> Items { get; }

        void Save(T entity);
        List<T> FindById(int id);
    }
}
