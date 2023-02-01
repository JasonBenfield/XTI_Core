using Microsoft.EntityFrameworkCore;

namespace XTI_Core.EF;

public sealed class EfDataRepository<T> : DataRepository<T> where T : class
{
    private readonly UnitOfWork unitOfWork;
    private readonly DbContext dbContext;
    private readonly DbSet<T> dbSet;

    public EfDataRepository(DbContext dbContext)
    {
        unitOfWork = new UnitOfWork(dbContext);
        this.dbContext = dbContext;
        dbSet = dbContext.Set<T>();
    }

    public Task<int> Create(T record) => Create(record, default);

    public Task<int> Create(T record, CancellationToken ct)
    {
        dbSet.Add(record);
        return dbContext.SaveChangesAsync(ct);
    }

    public Task<int> Delete(T record) => Delete(record, default);

    public Task<int> Delete(T record, CancellationToken ct)
    {
        dbSet.Remove(record);
        return dbContext.SaveChangesAsync(ct);
    }

    public Task<int> DeleteRange(params T[] records) => DeleteRange(records, default);

    public Task<int> DeleteRange(T[] records, CancellationToken ct)
    {
        dbSet.RemoveRange(records);
        return dbContext.SaveChangesAsync(ct);
    }

    public Task Reload(T record) => Reload(record, default);

    public Task Reload(T record, CancellationToken ct) => dbContext.Entry(record).ReloadAsync(ct);

    public IQueryable<T> Retrieve() => dbSet;

    public Task Transaction(Func<Task> action) => unitOfWork.Execute(action);

    public Task<TResult> Transaction<TResult>(Func<Task<TResult>> action) => unitOfWork.Execute(action);

    public Task<int> Update(T record, Action<T> a) => Update(record, a, default);

    public Task<int> Update(T record, Action<T> a, CancellationToken ct)
    {
        dbSet.Update(record);
        a(record);
        return dbContext.SaveChangesAsync(ct);
    }
}