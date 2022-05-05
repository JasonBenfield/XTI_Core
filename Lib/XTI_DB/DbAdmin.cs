using Microsoft.EntityFrameworkCore;

namespace XTI_DB;

public sealed class DbAdmin
{
    private readonly DbContext db;

    public DbAdmin(DbContext db)
    {
        this.db = db;
    }

    public Task BackupTo(string backupFilePath) => new DbBackup(db).Run(backupFilePath);

    public Task RestoreFrom(string backupFilePath) => new DbBackup(db).Run(backupFilePath);

    public Task Reset() => new DbReset(db).Run();
}
