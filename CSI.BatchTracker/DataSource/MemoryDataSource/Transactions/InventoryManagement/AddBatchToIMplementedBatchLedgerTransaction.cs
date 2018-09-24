using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.DataSource.MemoryDataSource.Transactions.InventoryManagement
{
    public class AddBatchToImplementedBatchLedgerTransaction : MemoryDataSourceTransaction
    {
        Entity<LoggedBatch> entity;
        MemoryStore store;

        public AddBatchToImplementedBatchLedgerTransaction(Entity<LoggedBatch> entity, MemoryStore store)
        {
            this.entity = entity;
            this.store = store;
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
