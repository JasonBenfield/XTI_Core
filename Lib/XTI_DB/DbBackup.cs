using Microsoft.EntityFrameworkCore;

namespace XTI_DB;

internal sealed class DbBackup
{
    private readonly DbContext dbContext;

    public DbBackup(DbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public Task Run(XtiDbName dbName, string backupFilePath)
    {
        FormattableString commandText =
            $"BACKUP DATABASE {dbName.Value} TO DISK = {backupFilePath}";
        return dbContext.Database.ExecuteSqlInterpolatedAsync(commandText);
    }
}