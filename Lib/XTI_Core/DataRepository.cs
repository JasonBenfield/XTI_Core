namespace XTI_Core;

public interface DataRepository<T> where T : class
{
    Task Create(T record);
    Task Reload(T record);
    IQueryable<T> Retrieve();
    Task Update(T record, Action<T> a);
    Task Delete(T record);
    Task DeleteRange(params T[] record);
    Task Transaction(Func<Task> action);

}