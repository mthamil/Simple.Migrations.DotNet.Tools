using CommandLine.Core.Hosting.Abstractions;
using CommandLine.Core.CommandLineUtils;
using CommandLine.Core.CommandLineUtils.Options;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Simple.Migrations.Tools.DotNet.Commands;
using Simple.Migrations.Tools.DotNet.Migrations;
using Simple.Migrations.Tools.DotNet.Resources;
using Simple.Migrations.Tools.DotNet.Utilities.IO;
using Simple.Migrations.Tools.DotNet.Utilities.Localization;

namespace Simple.Migrations.Tools.DotNet
{
    public class Startup : IStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization()
                    .AddTransient<CommandDescriptionProvider<OptionDescriptions>>();

            services.AddSingleton<IFileSystem, PhysicalFileSystem>();

            services.AddSingleton<SimpleMigrations.ILogger, SimpleMigrations.Console.ConsoleLogger>();

            services.AddScoped<IMigratorFactory, MigratorFactory>();
            services.AddScoped<IProjectBuilder, ProjectBuilder>();
            services.AddScoped<IProjectInfoDiscoverer, ProjectInfoDiscoverer>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCommands(c =>
            {
                c.Name = "dotnet sm";
                c.VersionOptionFromAssemblyAttributes(typeof(Startup).Assembly);

                c.Options(opts =>
                    opts.Option("-c|--configuration", inherited: true)
                        .Option("-f|--framework", inherited: true)
                        .Option("-p|--project", inherited: true)
                        .Option("--no-build", type: CommandOptionType.NoValue, inherited: true)
                        .Option("--assembly", inherited: true)
                        .Option("--connection-string", inherited: true)
                        .Option("--connection-string-name", inherited: true)
                        .WithDescriptionsFrom(s => s.GetService<CommandDescriptionProvider<OptionDescriptions>>()));

                c.Command("database", d =>
                {
                    d.Command<MigrateCommand>("migrate", m =>
                    {
                        m.Argument("migration", "The migration version to migrate to. If not provided, the latest version is chosen by default");
                        m.Option("--by-name", "Whether to find a migration by name or number.", CommandOptionType.NoValue);
                    });
                });

                c.Command("migrations", m =>
                    m.Command<ListCommand>("list"));
            });
        }
    }
}
