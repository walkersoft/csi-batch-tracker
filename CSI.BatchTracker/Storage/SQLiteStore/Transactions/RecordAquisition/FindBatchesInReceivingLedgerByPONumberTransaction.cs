using CSI.BatchTracker.Domain.NativeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Storage.SQLiteStore.Transactions.RecordAquisition
{
    public sealed class FindBatchesInReceivingLedgerByPONumberTransaction : SQLiteDataSourceTransaction
    {
        SQLiteStoreContext store;
        int targetPo;

        public FindBatchesInReceivingLedgerByPONumberTransaction(int targetPo, SQLiteStoreContext store)
        {
            this.store = store;
            this.targetPo = targetPo;
        }

        public override void Execute()
        {
            string query = "SELECT * FROM ReceivedBatches WHERE PONumber = ?";
            List<object> parameters = new List<object> { targetPo };
            store.ExecuteReader(typeof(ReceivedBatch), query, parameters);
            Results = store.Results;
        }
    }
}
