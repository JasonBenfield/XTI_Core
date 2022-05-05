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
        FormattableString commandText =
            $"BACKUP DATABASE {dbName} TO DISK = {backupFilePath}";
        return dbContext.Database.ExecuteSqlInterpolatedAsync(commandText);
    }
}