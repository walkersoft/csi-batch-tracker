using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Storage.Contracts
{
    public interface IPersistenceManager<TContextType> where TContextType : class
    {
        TContextType Context { get; set; }
        string StoredContextLocation { get; set; }
        void SaveDataSource();
        void LoadDataSource();
        void ClearDataSource();
    }
}
