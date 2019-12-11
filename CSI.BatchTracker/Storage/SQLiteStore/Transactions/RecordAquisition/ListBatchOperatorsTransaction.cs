using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Storage.SQLiteStore.Transactions.RecordAquisition
{
    public sealed class ListBatchOperatorsTransaction : SQLiteDataSourceTransaction
    {
        SQLiteStoreContext store;

        public ListBatchOperatorsTransaction(SQLiteStoreContext store)
        {
            this.store = store;
        }

        public override void Execute()
        {
            string query = "SELECT * FROM BatchOperators";
            store.ExecuteReader(typeof(BatchOperator), query);
            Results = store.Results;
        }
    }
}
