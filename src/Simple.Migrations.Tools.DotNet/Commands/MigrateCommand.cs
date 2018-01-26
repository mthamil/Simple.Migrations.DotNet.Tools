using McMaster.Extensions.CommandLineUtils;
using Simple.Migrations.Tools.DotNet.Migrations;
using Simple.Migrations.Tools.DotNet.Utilities;
using System;
using System.Linq;
using System.Reflection;

namespace Simple.Migrations.Tools.DotNet.Commands
{
    public class MigrateCommand : CommandLineApplication
    {
        private readonly CommandArgument _migration;
        private readonly CommandOption _byName;
        private readonly CommandOption _assembly;
        private readonly CommandOption _connectionString;
        private readonly IMigratorFactory _migratorFactory;
        private readonly IConsole _console;

        public MigrateCommand(IMigratorFactory migratorFactory, IConsole console)
        {
            _migratorFactory = migratorFactory ?? throw new ArgumentNullException(nameof(migratorFactory));
            _console = console ?? throw new ArgumentNullException(nameof(console));

            Name = "migrate";
            Description = "Migrates a database.";
            _migration = Argument("migration", "The migration version to migrate to. If not provided, the latest version is chosen by default");
            _byName = Option("--by-name", "Whether to find a migration by name or number.", CommandOptionType.NoValue);
            _assembly = Option("--assembly", "The assembly containing migrations.", CommandOptionType.SingleValue);
            _connectionString = Option("--connection-string", "The database connection string.", CommandOptionType.SingleValue);

            OnExecute(() => Execute());
        }

        private int Execute()
        {
            var migrator = _migratorFactory.Create(new MigratorOptions(
                Assembly.LoadFrom(_assembly.Value()),
                _connectionString.Value()));

            if (_migration.Value == null)
            {
                if (migrator.CurrentMigration.Version == migrator.LatestMigration.Version)
                    _console.WriteLine("Database is already at the latest version.");
                else
                    migrator.MigrateToLatest();
            }
            else
            {
                var migrationVersion = _byName.IsOn()
                    ? migrator.Migrations.SingleOrDefault(m => _migration.Value.Equals(m.TypeInfo?.Name, StringComparison.OrdinalIgnoreCase))?.Version
                    : Int64.Parse(_migration.Value);

                if (migrationVersion.HasValue)
                {
                    if (migrator.CurrentMigration.Version == migrationVersion)
                        _console.WriteLine("Database is already at this version.");
                    else
                        migrator.MigrateTo(migrationVersion.Value);
                }
                else
                {
                    _console.WriteLine("Migration could not be found.");
                }
            }

            return 0;
        }
    }
}
