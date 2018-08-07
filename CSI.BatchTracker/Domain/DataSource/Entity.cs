using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSI.BatchTracker.Domain.DataSource
{
    public class Entity<T> where T : class
    {
        public int SystemId { get; private set; }
        public T NativeModel { get; set; }

        public Entity(T nativeModel)
        {
            SystemId = 0;
            NativeModel = nativeModel;
        }

        public Entity(int systemId, T nativeModel)
        {
            SystemId = systemId;
            NativeModel = nativeModel;
        }
    }
}
