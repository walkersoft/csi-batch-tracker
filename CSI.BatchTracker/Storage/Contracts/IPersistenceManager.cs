namespace CSI.BatchTracker.Storage.Contracts
{
    public interface IPersistenceManager<TContextType> where TContextType : class
    {
        TContextType Context { get; set; }
        string StoredContextLocation { get; set; }
        void SaveDataSource();
        void LoadDataSource();
        void ClearDataSource();
    }
}
