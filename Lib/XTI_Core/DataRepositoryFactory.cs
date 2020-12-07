namespace XTI_Core
{
    public interface DataRepositoryFactory<T> where T : class
    {
        DataRepository<T> Create();
    }
}
