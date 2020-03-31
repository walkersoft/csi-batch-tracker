using CSI.BatchTracker.Storage.Contracts;

namespace CSI.BatchTracker.Domain.DataSource
{
    public sealed class Entity<T> : IEntity where T : class
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
