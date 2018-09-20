using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.DataSource
{
    public interface ITransaction
    {
        bool CanExecute { get; }
        void Execute();
    }
}
