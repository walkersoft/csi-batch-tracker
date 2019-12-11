using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Storage.SQLiteStore.PersistenceModels
{
    public class SQLiteBatchOperator
    {
        public int SystemId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
