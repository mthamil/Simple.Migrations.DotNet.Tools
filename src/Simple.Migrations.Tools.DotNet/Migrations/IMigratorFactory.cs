using SimpleMigrations;

namespace Simple.Migrations.Tools.DotNet.Migrations
{
    public interface IMigratorFactory
    {
        ISimpleMigrator Create(MigratorOptions options);
    }
}
