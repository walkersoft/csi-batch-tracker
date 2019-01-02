using System.Collections.Generic;

namespace CSI.BatchTracker.Storage.Contracts
{
    public interface ITransaction
    {
        bool CanExecute { get; }
        List<IEntity> Results { get; }
        void Execute();
    }
}
