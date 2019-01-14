using System.Collections.Generic;

namespace CSI.BatchTracker.Storage.Contracts
{
    public interface ITransaction
    {
        List<IEntity> Results { get; }
        void Execute();
    }
}
