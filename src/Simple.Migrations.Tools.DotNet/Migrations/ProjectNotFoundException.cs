using Simple.Migrations.Tools.DotNet.Commands.Options;

namespace Simple.Migrations.Tools.DotNet.Migrations
{
    public class ProjectNotFoundException : CommandOptionException
    {
        public ProjectNotFoundException(string message) 
            : base("project", message) { }
    }
}
