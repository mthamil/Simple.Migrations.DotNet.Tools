using Simple.Migrations.Tools.DotNet.Commands.Options;

namespace Simple.Migrations.Tools.DotNet.Migrations
{
    public class InvalidMigrationException : CommandArgumentException
    {
        public InvalidMigrationException(string invalidName)
            : base("migration", $"Invalid migration '{invalidName}'.")
        {
        }
    }
}
