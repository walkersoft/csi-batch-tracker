using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSI.BatchTracker.Contracts;

namespace CSI.BatchTracker.Storage.MemoryStore.Transactions
{
    abstract public class MemoryDataSourceTransaction : ITransaction
    {
        bool canExecute = true;

        public virtual bool CanExecute
        {
            get { return canExecute; }
            protected set { canExecute = value; }
        }

        public List<IEntity> Results { get; protected set; }

        public MemoryDataSourceTransaction()
        {
            Results = new List<IEntity>();
        }

        public abstract void Execute();
    }
}
