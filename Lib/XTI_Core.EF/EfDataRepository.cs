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

    public Task Create(T record)
    {
        dbSet.Add(record);
        return dbContext.SaveChangesAsync();
    }

    public Task Delete(T record)
    {
        dbSet.Remove(record);
        return dbContext.SaveChangesAsync();
    }

    public IQueryable<T> Retrieve() => dbSet;

    public Task Transaction(Func<Task> action) => unitOfWork.Execute(action);

    public Task Update(T record, Action<T> a)
    {
        dbSet.Update(record);
        a(record);
        return dbContext.SaveChangesAsync();
    }
}