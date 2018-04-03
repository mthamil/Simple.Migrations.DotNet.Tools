using CommandLine.Core.Hosting.CommandLineUtils.Options;
using CommandLine.Core.Hosting.CommandLineUtils.Utilities;
using McMaster.Extensions.CommandLineUtils;
using Simple.Migrations.Tools.DotNet.Migrations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.Migrations.Tools.DotNet.Commands
{
    public class MigrateCommand : CommandLineApplication
    {
        private readonly IConsole _console;
        private readonly IMigratorFactory _migratorFactory;

        private readonly CommandArgument _migration;
        private readonly CommandOption _byName;
        private readonly ISharedOptions _commonOptions;

        public MigrateCommand(IConsole console,
                              ISharedOptions commonOptions,
                              IMigratorFactory migratorFactory)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _commonOptions = commonOptions ?? throw new ArgumentNullException(nameof(commonOptions));
            _migratorFactory = migratorFactory ?? throw new ArgumentNullException(nameof(migratorFactory));

            Name = "migrate";
            Description = "Migrates a database.";

            _migration = Argument("migration", "The migration version to migrate to. If not provided, the latest version is chosen by default");
            _byName = Option("--by-name", "Whether to find a migration by name or number.", CommandOptionType.NoValue);

            OnExecute(() => ExecuteAsync());
        }

        private async Task<int> ExecuteAsync()
        {
            var migrator = await _migratorFactory.CreateAsync(_commonOptions.Map<MigrationOptions>());

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
                    : Int64.TryParse(_migration.Value, out var result)
                        ? result
                        : throw new InvalidMigrationException(_migration.Value);

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
