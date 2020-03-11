using CSI.BatchTracker.Domain.NativeModels;
using System.Collections.Generic;

namespace CSI.BatchTracker.Storage.SQLiteStore.Transactions.BatchOperators
{
    public sealed class CheckIfOperatorBelongsToRelatedEntitiesCommand : SQLiteDataSourceTransaction
    {
        int id;
        SQLiteStoreContext store;

        public CheckIfOperatorBelongsToRelatedEntitiesCommand(int id, SQLiteStoreContext store)
        {
            this.id = id;
            this.store = store;
        }

        public override void Execute()
        {
            string query = "SELECT * FROM ReceivedBatches WHERE ReceivingOperatorId = ?";
            List<object> parameters = new List<object>() { id };
            store.ExecuteReader(typeof(ReceivedBatch), query, parameters);
            Results = store.Results;

            if (Results.Count > 0)
            {
                return;
            }

            query = "SELECT * FROM ImplementedBatches WHERE ImplementingOperatorId = ?";
            store.ExecuteReader(typeof(LoggedBatch), query, parameters);
            Results = store.Results;
        }
    }
}
