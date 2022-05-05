using Microsoft.EntityFrameworkCore;

namespace XTI_DB;

public sealed class DbAdmin
{
    private readonly DbContext db;
    private readonly XtiDbName dbName;

    public DbAdmin(DbContext db, XtiDbName dbName)
    {
        this.db = db;
        this.dbName = dbName;
    }

    public Task Backup(string backupFilePath) => new DbBackup(db).Run(dbName, backupFilePath);

    public Task Restore(string backupFilePath) => new DbBackup(db).Run(dbName, backupFilePath);

    public Task Reset() => new DbReset(db).Run();
}
