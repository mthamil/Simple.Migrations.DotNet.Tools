using CommandLine.Core.Hosting.CommandLineUtils.Options;
using CommandLine.Core.Hosting.CommandLineUtils.Utilities;
using McMaster.Extensions.CommandLineUtils;
using Simple.Migrations.Tools.DotNet.Migrations;
using System;
using System.Threading.Tasks;

namespace Simple.Migrations.Tools.DotNet.Commands
{
    public class ListCommand : CommandLineApplication
    {
        private readonly ISharedOptions _commonOptions;
        private readonly IMigratorFactory _migratorFactory;
        private readonly IConsole _console;

        public ListCommand(IConsole console, ISharedOptions commonOptions, IMigratorFactory migratorFactory)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _commonOptions = commonOptions ?? throw new ArgumentNullException(nameof(commonOptions));
            _migratorFactory = migratorFactory ?? throw new ArgumentNullException(nameof(migratorFactory));

            Name = "list";
            Description = "Lists migrations.";

            OnExecute(() => ExecuteAsync());
        }

        private async Task<int> ExecuteAsync()
        {
            var migrator = await _migratorFactory.CreateAsync(_commonOptions.Map<MigrationOptions>());

            foreach (var migration in migrator.Migrations)
            {
                var marker = migration == migrator.CurrentMigration ? "*" : " ";
                _console.WriteLine($"{marker} {migration.Version}: {migration.FullName}");
            }

            return 0;
        }
    }
}
