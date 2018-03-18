using SimpleMigrations;
using System.Threading.Tasks;

namespace Simple.Migrations.Tools.DotNet.Migrations
{
    public interface IMigratorFactory
    {
        Task<ISimpleMigrator> CreateAsync(MigrationOptions options);
    }
}
