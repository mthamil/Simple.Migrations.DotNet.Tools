using CommandLine.Core.Hosting.Abstractions;
using CommandLine.Core.Hosting.CommandLineUtils;
using CommandLine.Core.Hosting.CommandLineUtils.Options;
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

            services.AddCommands(cmds =>
                cmds.Base<DatabaseCommand>()
                    .Child<MigrateCommand>()
                    .Base<MigrationsCommand>()
                    .Child<ListCommand>());

            services.AddCommonOptions(opts =>
                opts.Option("-c|--configuration")
                    .Option("-f|--framework")
                    .Option("-p|--project")
                    .Option("--no-build", type: CommandOptionType.NoValue)
                    .Option("--assembly")
                    .Option("--connection-string")
                    .Option("--connection-string-name")
                    .WithDescriptionsFrom(s => s.GetService<CommandDescriptionProvider<OptionDescriptions>>()));

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
            });
        }
    }
}
