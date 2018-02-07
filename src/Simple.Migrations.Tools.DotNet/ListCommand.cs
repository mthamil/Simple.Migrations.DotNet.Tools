using McMaster.Extensions.CommandLineUtils;
using Simple.Migrations.Tools.DotNet.Migrations;
using System;
using System.Reflection;

namespace Simple.Migrations.Tools.DotNet
{

    public class ListCommand : CommandLineApplication
    {
        private readonly CommandOption _assembly;
        private readonly CommandOption _connectionString;
        private readonly IMigratorFactory _migratorFactory;
        private readonly IConsole _console;

        public ListCommand(IMigratorFactory migratorFactory, IConsole console)
        {
            _migratorFactory = migratorFactory ?? throw new ArgumentNullException(nameof(migratorFactory));
            _console = console ?? throw new ArgumentNullException(nameof(console));

            Name = "list";
            Description = "Lists migrations.";
            _assembly = Option("--assembly", "The assembly containing migrations.", CommandOptionType.SingleValue);
            _connectionString = Option("--connection-string", "The database connection string.", CommandOptionType.SingleValue);

            OnExecute(() => Execute());
        }

        private int Execute()
        {
            var migrator = _migratorFactory.Create(new MigratorOptions(
                Assembly.LoadFrom(_assembly.Value()),
                _connectionString.Value()));

            foreach (var migration in migrator.Migrations)
            {
                var marker = migration == migrator.CurrentMigration ? "*" : " ";
                _console.WriteLine($"{marker} {migration.Version}: {migration.FullName}");
            }

            return 0;
        }
    }
}
