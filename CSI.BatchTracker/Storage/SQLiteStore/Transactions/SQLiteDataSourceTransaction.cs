using CSI.BatchTracker.Storage.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Storage.SQLiteStore.Transactions
{
    public abstract class SQLiteDataSourceTransaction : ITransaction
    {
        public List<IEntity> Results { get; protected set; }

        public SQLiteDataSourceTransaction()
        {
            Results = new List<IEntity>();
        }

        public abstract void Execute();
    }
}
