namespace XTI_Core
{
    public interface DataRepositoryFactory
    {
        DataRepository<T> Create<T>() where T : class;
    }
}
