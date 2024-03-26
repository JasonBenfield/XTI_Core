using Microsoft.EntityFrameworkCore;

namespace XTI_DB;

public sealed class DbAdmin<TDbContext>
    where TDbContext : DbContext
{
    private readonly IDbContextFactory<TDbContext> dbFactory;

    public DbAdmin(IDbContextFactory<TDbContext> db)
    {
        this.dbFactory = db;
    }

    public async Task BackupTo(string backupFilePath)
    {
        using (var db = dbFactory.CreateDbContext())
        {
            await new DbBackup(dbFactory.CreateDbContext()).Run(backupFilePath);
        }
    }

    public Task RestoreFrom(string backupFilePath) =>
        new DbRestore<TDbContext>(dbFactory).Run(backupFilePath);

    public async Task Reset()
    {
        using (var db = dbFactory.CreateDbContext())
        {
            await new DbReset(db).Run();
        }
    }

    public async Task Update()
    {
        using (var db = dbFactory.CreateDbContext())
        {
            await db.Database.MigrateAsync();
        }
    }
}
