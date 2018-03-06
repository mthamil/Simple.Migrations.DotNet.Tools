using System.Threading.Tasks;

namespace Simple.Migrations.Tools.DotNet.Migrations
{
    public interface IMigrationProjectInfoLocator
    {
        Task<MigratorOptions> LocateAsync(
            bool shouldBuildIfNeeded,
            string configuration,
            string targetFramework,
            string project,
            string assembly,
            string connectionString,
            string connectionStringName);
    }
}