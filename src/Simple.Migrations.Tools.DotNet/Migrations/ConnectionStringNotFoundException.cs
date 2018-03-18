using Simple.Migrations.Tools.DotNet.Commands.Options;

namespace Simple.Migrations.Tools.DotNet.Migrations
{
    public class ConnectionStringNotFoundException : CommandOptionException
    {
        public ConnectionStringNotFoundException(string message) 
            : base("connection-string", message) { }
    }
}
