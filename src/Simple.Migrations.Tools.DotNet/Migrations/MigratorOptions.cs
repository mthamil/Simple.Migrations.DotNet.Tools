using System.Reflection;

namespace Simple.Migrations.Tools.DotNet.Migrations
{
    public class MigratorOptions
    {
        public MigratorOptions(Assembly migrationsAssembly, 
                               string connectionString)
        {
            MigrationsAssembly = migrationsAssembly;
            ConnectionString = connectionString;
        }

        public Assembly MigrationsAssembly { get; }

        public string ConnectionString { get; }
    }
}
