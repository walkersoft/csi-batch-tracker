using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.DataSource.MemoryDataSource.Transactions
{
    abstract public class MemoryDataSourceTransaction : ITransaction
    {
        bool canExecute = true;

        public virtual bool CanExecute
        {
            get { return canExecute; }
            protected set { canExecute = value; }
        }

        public abstract void Execute();
    }
}
