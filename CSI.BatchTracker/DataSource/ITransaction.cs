using CSI.BatchTracker.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.DataSource
{
    public interface ITransaction
    {
        bool CanExecute { get; }
        List<IEntity> Results { get; }
        void Execute();
    }
}
