using McMaster.Extensions.CommandLineUtils;

namespace Simple.Migrations.Tools.DotNet.Commands
{
    public class MigrationsCommand : CommandLineApplication
    {
        public MigrationsCommand(ListCommand listCommand)
        {
            Name = "migrations";
            Description = "Migrates a database.";
            Commands.Add(listCommand);
            listCommand.Parent = this;
        }
    }
}
