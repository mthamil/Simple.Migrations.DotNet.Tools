using McMaster.Extensions.CommandLineUtils;
using Simple.Migrations.Tools.DotNet.Migrations;
using Simple.Migrations.Tools.DotNet.Utilities;
using System;
using System.Reflection;

namespace Simple.Migrations.Tools.DotNet.Commands
{

    public class ListCommand : CommandLineApplication
    {
        private readonly Lazy<CommandOption> _assembly;
        private readonly Lazy<CommandOption> _connectionString;
        private readonly IMigratorFactory _migratorFactory;
        private readonly IConsole _console;

        public ListCommand(IMigratorFactory migratorFactory, IConsole console)
        {
            _migratorFactory = migratorFactory ?? throw new ArgumentNullException(nameof(migratorFactory));
            _console = console ?? throw new ArgumentNullException(nameof(console));

            Name = "list";
            Description = "Lists migrations.";
            _assembly = this.InheritedOption("assembly");
            _connectionString = this.InheritedOption("connection-string");

            OnExecute(() => Execute());
        }

        private int Execute()
        {
            var migrator = _migratorFactory.Create(new MigratorOptions(
                Assembly.LoadFrom(_assembly.Value.Value()),
                _connectionString.Value.Value()));

            foreach (var migration in migrator.Migrations)
            {
                var marker = migration == migrator.CurrentMigration ? "*" : " ";
                _console.WriteLine($"{marker} {migration.Version}: {migration.FullName}");
            }

            return 0;
        }
    }
}
