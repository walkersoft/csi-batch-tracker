using CSI.BatchTracker.Domain.DataSource;
using CSI.BatchTracker.Domain.NativeModels;
using CSI.BatchTracker.Storage.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Storage.MemoryStore
{
    public class MemoryStorePersistenceManager : IPersistenceManager<MemoryStoreContext>
    {
        public MemoryStoreContext Context { get; set; }

        public string StoredContextLocation { get; set; }

        public MemoryStorePersistenceManager(string storedContextLocation)
        {
            Context = new MemoryStoreContext();
            StoredContextLocation = storedContextLocation;
        }

        public void SaveDataSource()
        {
            using (FileStream stream = new FileStream(StoredContextLocation, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, Context);
            }
        }

        public void LoadDataSource()
        {
            using (FileStream stream = new FileStream(StoredContextLocation, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStoreContext loaded = formatter.Deserialize(stream) as MemoryStoreContext;
                Context.BatchOperators = new Dictionary<int, Entity<BatchOperator>>(loaded.BatchOperators);
                Context.CurrentInventory = new Dictionary<int, Entity<InventoryBatch>>(loaded.CurrentInventory);
                Context.ReceivingLedger = new Dictionary<int, Entity<ReceivedBatch>>(loaded.ReceivingLedger);
                Context.ImplementedBatchLedger = new Dictionary<int, Entity<LoggedBatch>>(loaded.ImplementedBatchLedger);
            }
        }

        public void ClearDataSource()
        {
            Context = new MemoryStoreContext();
        }
    }
}
