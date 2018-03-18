using Simple.Migrations.Tools.DotNet.Commands.Options;

namespace Simple.Migrations.Tools.DotNet.Migrations
{
    public class TargetFrameworkNotFoundException : CommandOptionException
    {
        public TargetFrameworkNotFoundException(string message)
            : base("framework", message) { }
    }
}
