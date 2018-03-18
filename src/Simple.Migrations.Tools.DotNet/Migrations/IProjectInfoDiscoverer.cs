using System.Threading.Tasks;

namespace Simple.Migrations.Tools.DotNet.Migrations
{
    public interface IProjectInfoDiscoverer
    {
        Task<ProjectInfo> DiscoverAsync(
            bool shouldBuildIfNeeded,
            string configuration,
            string targetFramework,
            string project,
            string assembly,
            string connectionString,
            string connectionStringName);
    }
}