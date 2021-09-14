using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace XTI_DB
{
    public sealed class DbBackup
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
}
