using CSI.BatchTracker.Storage.Contracts;
using System.Collections.Generic;

namespace CSI.BatchTracker.Storage.MemoryStore.Transactions
{
    abstract public class MemoryDataSourceTransaction : ITransaction
    {
        public List<IEntity> Results { get; protected set; }

        public MemoryDataSourceTransaction()
        {
            Results = new List<IEntity>();
        }

        public abstract void Execute();
    }
}
