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

        public MemoryStorePersistenceManager(MemoryStoreContext context, string storedContextLocation)
        {
            Context = context;
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
                Context = formatter.Deserialize(stream) as MemoryStoreContext;
            }
        }

        public void ClearDataSource()
        {
            Context = new MemoryStoreContext();
        }
    }
}
