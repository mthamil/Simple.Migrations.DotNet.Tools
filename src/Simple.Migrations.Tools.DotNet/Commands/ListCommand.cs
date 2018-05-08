using McMaster.Extensions.CommandLineUtils;
using Simple.Migrations.Tools.DotNet.Migrations;
using System;
using System.Threading.Tasks;

namespace Simple.Migrations.Tools.DotNet.Commands
{
    public class ListCommand
    {
        private readonly IMigratorFactory _migratorFactory;
        private readonly IConsole _console;

        public ListCommand(IConsole console, IMigratorFactory migratorFactory)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _migratorFactory = migratorFactory ?? throw new ArgumentNullException(nameof(migratorFactory));
        }

        public MigrationOptions MigrationOptions { get; set; }

        public async Task<int> OnExecuteAsync()
        {
            var migrator = await _migratorFactory.CreateAsync(MigrationOptions);

            foreach (var migration in migrator.Migrations)
            {
                var marker = migration == migrator.CurrentMigration ? "*" : " ";
                _console.WriteLine($"{marker} {migration.Version}: {migration.FullName}");
            }

            return 0;
        }
    }
}
