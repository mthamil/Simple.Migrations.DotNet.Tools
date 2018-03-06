using McMaster.Extensions.CommandLineUtils;

namespace Simple.Migrations.Tools.DotNet.Commands
{
    public class DatabaseCommand : CommandLineApplication
    {
        public DatabaseCommand(MigrateCommand migrateCommand)
        {
            Name = "database";
            Description = "";
            Commands.Add(migrateCommand);
            migrateCommand.Parent = this;
        }
    }
}
