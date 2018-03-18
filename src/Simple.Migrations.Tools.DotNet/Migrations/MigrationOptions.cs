using Simple.Migrations.Tools.DotNet.Commands.Options;
using Simple.Migrations.Tools.DotNet.Utilities;
using SimpleMigrations;
using SimpleMigrations.DatabaseProvider;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Simple.Migrations.Tools.DotNet.Migrations
{

    public class MigrationOptions
    {
        public bool NoBuild { get; set; }
        public string Configuration { get; set; }
        public string Framework { get; set; }
        public string Project { get; set; }
        public string Assembly { get; set; }
        public string ConnectionString { get; set; }
        public string ConnectionStringName { get; set; }
    }
}
