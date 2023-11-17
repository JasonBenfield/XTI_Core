using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using XTI_Core;
using XTI_Core.Extensions;

namespace XTI_DB.Tests;

internal sealed class DbContextTest
{
    [Test]
    public async Task ShouldConnectToDB()
    {
        var sp = Setup("Development");
        var db = sp.GetRequiredService<TestDbContext>();
        var apps = await db.Set<AppEntity>().ToArrayAsync();
        apps.WriteToConsole();
    }

    private sealed class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppEntity>
            (
                a =>
                {
                    a.ToTable("Apps");
                }
            );
        }
    }

    private sealed class AppEntity
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";
    }

    private IServiceProvider Setup(string envName)
    {
        var hostBuilder = new XtiHostBuilder(XtiEnvironment.Parse(envName));
        hostBuilder.Services.AddConfigurationOptions<DbOptions>(DbOptions.DB);
        hostBuilder.Services.AddDbContext<TestDbContext>((sp, options) =>
        {
            var xtiEnv = sp.GetRequiredService<XtiEnvironment>();
            var dbOptions = sp.GetRequiredService<DbOptions>();
            dbOptions.IsAlwaysEncryptedEnabled = true;
            var connectionString = new XtiConnectionString
            (
                dbOptions, 
                new XtiDbName(xtiEnv.EnvironmentName, "Hub")
            );
            var connectionStringValue = connectionString.Value();
            options.UseSqlServer(connectionStringValue);
            if (xtiEnv.IsDevelopmentOrTest())
            {
                options.EnableSensitiveDataLogging();
            }
            else
            {
                options.EnableSensitiveDataLogging(false);
            }
        });
        var host = hostBuilder.Build();
        return host.Scope();
    }
}
