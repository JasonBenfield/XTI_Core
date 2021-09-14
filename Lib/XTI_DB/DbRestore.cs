using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace XTI_DB
{
    public sealed class DbRestore
    {
        private readonly DbContext dbContext;

        public DbRestore(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Run(XtiDbName dbName, string backupFilePath)
        {
            backupFilePath = backupFilePath.Replace("'", "''");
            var currentFiles = await retrieveCurrentFiles();
            var currentDataFile = currentFiles.First(f => !f.IsLog);
            var currentLogFile = currentFiles.First(f => f.IsLog);
            var backupFiles = await retrieveBackupFiles(backupFilePath);
            var backupDataFile = backupFiles.First(f => !f.IsLog);
            var backupLogFile = backupFiles.First(f => f.IsLog);
            await dbContext.Database.ExecuteSqlRawAsync
            (
                $"USE [master]\r\nalter database {dbName.Value} set single_user with rollback immediate"
            );
            await dbContext.Database.ExecuteSqlRawAsync
            (
                $"USE [master]\r\nRESTORE DATABASE {dbName.Value} FROM DISK = '{backupFilePath}' WITH MOVE '{backupDataFile.LogicalName}' TO '{currentDataFile.PhysicalName}',  MOVE '{backupLogFile.LogicalName}' TO '{currentLogFile.PhysicalName}',  NOUNLOAD, REPLACE"
            );
            await dbContext.Database.ExecuteSqlRawAsync
            (
                $"USE [master]\r\nalter database {dbName.Value} set multi_user"
            );
        }

        private async Task<IEnumerable<DatabaseFile>> retrieveCurrentFiles()
        {
            using (var command = dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "SELECT name, physical_name, type_desc FROM sys.database_files";
                command.CommandType = CommandType.Text;
                dbContext.Database.OpenConnection();
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

        private async Task<IEnumerable<DatabaseFile>> retrieveBackupFiles(string backupFilePath)
        {
            using (var command = dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = $"RESTORE FILELISTONLY FROM DISK = '{backupFilePath}'";
                command.CommandType = CommandType.Text;
                dbContext.Database.OpenConnection();
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
}
