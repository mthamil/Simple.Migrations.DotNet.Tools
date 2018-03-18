using System.Reflection;

namespace Simple.Migrations.Tools.DotNet.Migrations
{
    public class ProjectInfo
    {
        public ProjectInfo(Assembly projectAssembly, string connectionString)
        {
            ProjectAssembly = projectAssembly;
            ConnectionString = connectionString;
        }

        public Assembly ProjectAssembly { get; }

        public string ConnectionString { get; }
    }
}
