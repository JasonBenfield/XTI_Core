using Microsoft.EntityFrameworkCore;

namespace XTI_DB;

public sealed class DbReset
{
    private readonly DbContext db;

    public DbReset(DbContext db)
    {
        this.db = db;
    }

    public async Task Run()
    {
        await db.Database.ExecuteSqlRawAsync
        (
            @"
exec sp_MSForEachTable 'IF OBJECT_ID(''?'') <> ISNULL(OBJECT_ID(''[dbo].[__EFMigrationsHistory]''),0) ALTER TABLE ? NOCHECK CONSTRAINT all';

exec sp_MSForEachTable '
    set rowcount 0; 
    SET QUOTED_IDENTIFIER ON; 
    IF OBJECT_ID(''?'') <> ISNULL(OBJECT_ID(''[dbo].[__EFMigrationsHistory]''),0) 
        DELETE FROM ?;';

exec sp_MSForEachTable 'IF OBJECT_ID(''?'') <> ISNULL(OBJECT_ID(''[dbo].[__EFMigrationsHistory]''),0) ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all';

exec sp_MSForEachTable 'IF OBJECT_ID(''?'') <> ISNULL(OBJECT_ID(''[dbo].[__EFMigrationsHistory]''),0) DBCC CHECKIDENT(''?'', RESEED, 0)';
"
        );
    }
}