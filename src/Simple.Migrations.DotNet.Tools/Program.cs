using SimpleMigrations;
using SimpleMigrations.Console;
using SimpleMigrations.DatabaseProvider;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Simple.Migrations.DotNet.Tools
{
    class Program
    {
        public static void Main(string[] args)
        {
            AssemblyMigrationProvider migrationAssembly = null;
            DbConnection connection = null;
            var provider = new MssqlDatabaseProvider(connection) { SchemaName = "dbo" };
            var migrator = new SimpleMigrator(migrationAssembly, provider);
            migrator.Load();

            var runner = new ConsoleRunner(migrator);


        }
    }
}
