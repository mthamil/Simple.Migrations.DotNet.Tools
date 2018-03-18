using Simple.Migrations.Tools.DotNet.Utilities;
using SimpleMigrations;
using SimpleMigrations.DatabaseProvider;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Simple.Migrations.Tools.DotNet.Migrations
{
    public class MigratorFactory : IMigratorFactory
    {
        private readonly ILogger _migrationLogger;
        private readonly IProjectInfoDiscoverer _projectInfoDiscoverer;

        public MigratorFactory(ILogger migrationLogger, IProjectInfoDiscoverer projectInfoDiscoverer)
        {
            _migrationLogger = migrationLogger ?? throw new ArgumentNullException(nameof(migrationLogger));
            _projectInfoDiscoverer = projectInfoDiscoverer ?? throw new ArgumentNullException(nameof(projectInfoDiscoverer));
        }

        public async Task<ISimpleMigrator> CreateAsync(MigrationOptions options)
        {
            var projectInfo = await _projectInfoDiscoverer.DiscoverAsync(
                !options.NoBuild,
                options.Configuration,
                options.Framework,
                options.Project,
                options.Assembly,
                options.ConnectionString,
                options.ConnectionStringName);

            using (var loader = new DependencyAwareAssemblyLoader(projectInfo.ProjectAssembly))
            {
                var migrationProvider = new AssemblyMigrationProvider(projectInfo.ProjectAssembly);
                var connection = new SqlConnection(projectInfo.ConnectionString);
                var dbProvider = new MssqlDatabaseProvider(connection) { SchemaName = "dbo" };
                var migrator = new SimpleMigrator(migrationProvider, dbProvider, _migrationLogger);
                migrator.Load();
                return migrator;
            }
        }
    }
}
