using Microsoft.EntityFrameworkCore;

namespace XTI_DB;

internal sealed class DbBackup
{
    private readonly DbContext dbContext;

    public DbBackup(DbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public Task Run(string backupFilePath)
    {
        var dbName = dbContext.Database.GetDbConnection().Database;
        return dbContext.Database.ExecuteSqlInterpolatedAsync
        (
            $"backup database {dbName} TO DISK = {backupFilePath}"
        );
    }
}