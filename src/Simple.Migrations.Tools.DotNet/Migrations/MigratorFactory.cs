using Simple.Migrations.Tools.DotNet.Utilities;
using SimpleMigrations;
using SimpleMigrations.Console;
using SimpleMigrations.DatabaseProvider;
using System.Data.SqlClient;

namespace Simple.Migrations.Tools.DotNet.Migrations
{
    public class MigratorFactory : IMigratorFactory
    {
        public ISimpleMigrator Create(MigratorOptions options)
        {
            using (var loader = new DependencyAwareAssemblyLoader(options.MigrationsAssembly))
            {
                var migrationProvider = new AssemblyMigrationProvider(options.MigrationsAssembly);
                var connection = new SqlConnection(options.ConnectionString);
                var dbProvider = new MssqlDatabaseProvider(connection) { SchemaName = "dbo" };
                var migrator = new SimpleMigrator(migrationProvider, dbProvider, new ConsoleLogger());
                migrator.Load();
                return migrator;
            }
        }
    }
}
