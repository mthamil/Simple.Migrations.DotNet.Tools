using CommandLine.Core.CommandLineUtils.Utilities;
using McMaster.Extensions.CommandLineUtils;
using Simple.Migrations.Tools.DotNet.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.Migrations.Tools.DotNet.Commands
{
    public class MigrateCommand
    {
        private readonly IConsole _console;
        private readonly IMigratorFactory _migratorFactory;

        private readonly CommandArgument _migration;
        private readonly CommandOption _byName;
        private readonly IEnumerable<CommandOption> _options;

        public MigrateCommand(IConsole console,
                              IEnumerable<CommandOption> options,
                              IEnumerable<CommandArgument> arguments,
                              IMigratorFactory migratorFactory)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _migratorFactory = migratorFactory ?? throw new ArgumentNullException(nameof(migratorFactory));

            _migration = arguments.Single(a => a.Name == "migration");
            _byName = options.Single(o => o.LongName == "by-name");
        }

        public async Task<int> OnExecuteAsync()
        {
            var migrator = await _migratorFactory.CreateAsync(_options.Map<MigrationOptions>());

            if (_migration.Value == null)
            {
                if (migrator.CurrentMigration.Version == migrator.LatestMigration.Version)
                    _console.WriteLine("Database is already at the latest version.");
                else
                    migrator.MigrateToLatest();
            }
            else
            {
                var migrationVersion = _byName.HasValue()
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
