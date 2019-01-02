using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Storage.Contracts
{
    public interface IEntity
    {
        int SystemId { get; }
    }
}
