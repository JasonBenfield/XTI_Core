using Microsoft.EntityFrameworkCore;

namespace XTI_DB;

public sealed class DbAdmin<TDbContext>
    where TDbContext : DbContext
{
    private readonly TDbContext db;

    public DbAdmin(TDbContext db)
    {
        this.db = db;
    }

    public Task BackupTo(string backupFilePath) => new DbBackup(db).Run(backupFilePath);

    public Task RestoreFrom(string backupFilePath) => new DbBackup(db).Run(backupFilePath);

    public Task Reset() => new DbReset(db).Run();

    public Task Update() => db.Database.MigrateAsync();
}
