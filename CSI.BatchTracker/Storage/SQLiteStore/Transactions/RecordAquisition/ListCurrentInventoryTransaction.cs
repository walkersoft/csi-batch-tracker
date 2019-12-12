using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Storage.SQLiteStore.Transactions.RecordAquisition
{
    public sealed class ListCurrentInventoryTransaction : SQLiteDataSourceTransaction
    {
        SQLiteStoreContext store;

        public ListCurrentInventoryTransaction(SQLiteStoreContext store)
        {
            this.store = store;
        }

        public override void Execute()
        {
            string query = "SELECT * FROM InventoryBatches";
            store.ExecuteReader(typeof(InventoryBatch), query);
            Results = store.Results;
        }
    }
}
