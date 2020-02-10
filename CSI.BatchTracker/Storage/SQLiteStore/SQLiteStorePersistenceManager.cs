using CSI.BatchTracker.Storage.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Storage.SQLiteStore
{
    class SQLiteStorePersistenceManager : IPersistenceManager<SQLiteStoreContext>
    {
        public SQLiteStoreContext Context { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string StoredContextLocation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void ClearDataSource()
        {
            throw new NotImplementedException();
        }

        public void LoadDataSource()
        {
            throw new NotImplementedException();
        }

        public void SaveDataSource()
        {
            throw new NotImplementedException();
        }
    }
}
