namespace XTI_Core;

public interface DataRepository<T> where T : class
{
    Task<int> Create(T record);
    Task<int> Create(T record, CancellationToken ct);
    Task Reload(T record);
    Task Reload(T record, CancellationToken ct);
    IQueryable<T> Retrieve();
    Task<int> Update(T record, Action<T> a);
    Task<int> Update(T record, Action<T> a, CancellationToken ct);
    Task<int> Delete(T record);
    Task<int> Delete(T record, CancellationToken ct);
    Task<int> DeleteRange(T[] record);
    Task<int> DeleteRange(T[] record, CancellationToken ct);
    Task Transaction(Func<Task> action);
    Task<TResult> Transaction<TResult>(Func<Task<TResult>> action);

}