using CommandLine.Core.CommandLineUtils.Utilities;
using McMaster.Extensions.CommandLineUtils;
using Simple.Migrations.Tools.DotNet.Migrations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simple.Migrations.Tools.DotNet.Commands
{
    public class ListCommand
    {
        private readonly IEnumerable<CommandOption> _options;
        private readonly IMigratorFactory _migratorFactory;
        private readonly IConsole _console;

        public ListCommand(IConsole console, IEnumerable<CommandOption> options, IMigratorFactory migratorFactory)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _migratorFactory = migratorFactory ?? throw new ArgumentNullException(nameof(migratorFactory));
        }

        public async Task<int> OnExecuteAsync()
        {
            var migrator = await _migratorFactory.CreateAsync(_options.Map<MigrationOptions>());

            foreach (var migration in migrator.Migrations)
            {
                var marker = migration == migrator.CurrentMigration ? "*" : " ";
                _console.WriteLine($"{marker} {migration.Version}: {migration.FullName}");
            }

            return 0;
        }
    }
}
