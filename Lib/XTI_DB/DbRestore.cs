using Microsoft.EntityFrameworkCore;
using System.Data;

namespace XTI_DB;

internal sealed class DbRestore<TDbContext> where TDbContext : DbContext
{
    private readonly IDbContextFactory<TDbContext> dbContextFactory;

    public DbRestore(IDbContextFactory<TDbContext> dbContextFactory)
    {
        this.dbContextFactory = dbContextFactory;
    }

    public async Task Run(string backupFilePath)
    {
        using (var db1 = dbContextFactory.CreateDbContext())
        {
            var dbName = db1.Database.GetDbConnection().Database;
            backupFilePath = backupFilePath.Replace("'", "''");
            var currentFiles = await RetrieveCurrentFiles(db1);
            var currentDataFile = currentFiles.First(f => !f.IsLog);
            var currentLogFile = currentFiles.First(f => f.IsLog);
            var backupFiles = await RetrieveBackupFiles(db1, backupFilePath);
            var backupDataFile = backupFiles.First(f => !f.IsLog);
            var backupLogFile = backupFiles.First(f => f.IsLog);
            db1.Database.SetCommandTimeout(TimeSpan.FromHours(1));
            await db1.Database.ExecuteSqlAsync
            (
                $"alter database current set single_user with rollback immediate;"
            );
            await db1.Database.ExecuteSqlAsync($"use master");
            await db1.Database.ExecuteSqlInterpolatedAsync
            (
                $"restore database {dbName} FROM DISK = {backupFilePath} WITH MOVE {backupDataFile.LogicalName} TO {currentDataFile.PhysicalName},  MOVE {backupLogFile.LogicalName} TO {currentLogFile.PhysicalName},  NOUNLOAD, REPLACE;"
            );
        }
        using (var db2 = dbContextFactory.CreateDbContext())
        {
            await db2.Database.ExecuteSqlAsync
            (
                $"alter database current set multi_user;"
            );
        }
    }

    private async Task<IEnumerable<DatabaseFile>> RetrieveCurrentFiles(DbContext db)
    {
        using (var command = db.Database.GetDbConnection().CreateCommand())
        {
            command.CommandText = "SELECT name, physical_name, type_desc FROM sys.database_files";
            command.CommandType = CommandType.Text;
            db.Database.OpenConnection();
            using var result = await command.ExecuteReaderAsync();
            var entities = new List<DatabaseFile>();
            while (result.Read())
            {
                var type = result.GetString("type_desc");
                var databaseFile = new DatabaseFile
                (
                    result.GetString("name"),
                    result.GetString("physical_name"),
                    type == "LOG"
                );
                entities.Add(databaseFile);
            }
            return entities;
        }
    }

    private async Task<IEnumerable<DatabaseFile>> RetrieveBackupFiles(DbContext db, string backupFilePath)
    {
        backupFilePath = backupFilePath.Replace("'", "''");
        using var command = db.Database.GetDbConnection().CreateCommand();
        command.CommandText = $"RESTORE FILELISTONLY FROM DISK = '{backupFilePath}'";
        command.CommandType = CommandType.Text;
        db.Database.OpenConnection();
        using var result = await command.ExecuteReaderAsync();
        var entities = new List<DatabaseFile>();
        while (result.Read())
        {
            var type = result.GetString("Type");
            var databaseFile = new DatabaseFile
            (
                result.GetString("LogicalName"),
                result.GetString("PhysicalName"),
                type == "L"
            );
            entities.Add(databaseFile);
        }
        return entities;
    }

    private sealed class DatabaseFile
    {
        public DatabaseFile(string logicalName, string physicalName, bool isLog)
        {
            LogicalName = logicalName.Replace("'", "''");
            PhysicalName = physicalName.Replace("'", "''");
            IsLog = isLog;
        }

        public string LogicalName { get; }
        public string PhysicalName { get; }
        public bool IsLog { get; }
    }
}