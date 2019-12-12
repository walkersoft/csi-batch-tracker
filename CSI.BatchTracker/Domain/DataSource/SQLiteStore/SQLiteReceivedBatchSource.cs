using CSI.BatchTracker.Domain.DataSource.Contracts;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Storage.SQLiteStore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Domain.DataSource.SQLiteStore
{
    public sealed class SQLiteReceivedBatchSource : IReceivedBatchSource
    {
        public ObservableCollection<ReceivedBatch> ReceivedBatchRepository { get; private set; }
        public Dictionary<int, int> ReceivedBatchIdMappings { get; private set; }
        SQLiteStoreContext sqliteStore;
        IActiveInventorySource inventorySource;

        public SQLiteReceivedBatchSource(SQLiteStoreContext sqliteStore, IActiveInventorySource inventorySource)
        {
            this.sqliteStore = sqliteStore;
            this.inventorySource = inventorySource;
            ReceivedBatchRepository = new ObservableCollection<ReceivedBatch>();
            ReceivedBatchIdMappings = new Dictionary<int, int>();
        }

        public void DeleteReceivedBatch(int id)
        {
            throw new NotImplementedException();
        }

        public void FindAllReceivedBatches()
        {
            throw new NotImplementedException();
        }

        public ReceivedBatch FindReceivedBatchById(int id)
        {
            throw new NotImplementedException();
        }

        public void FindReceivedBatchesByDate(DateTime date)
        {
            throw new NotImplementedException();
        }

        public void FindReceivedBatchesByPONumber(int poNumber)
        {
            throw new NotImplementedException();
        }

        public EditablePurchaseOrder GetPurchaseOrderForEditing(int poNumber)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<ReceivedBatch> GetReceivedBatchesByBatchNumber(string batchNumber)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<ReceivedBatch> GetReceivedBatchesByPONumber(int poNumber)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<ReceivedBatch> GetReceivedBatchesbySpecificDate(DateTime specificDate)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<ReceivedBatch> GetReceivedBatchesWithinDateRange(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public void SaveReceivedBatch(ReceivedBatch receivedBatch)
        {
            throw new NotImplementedException();
        }

        public void UpdateReceivedBatch(int id, ReceivedBatch batch)
        {
            throw new NotImplementedException();
        }
    }
}
