using CommandLine.Core.Hosting.Abstractions;
using CommandLine.Core.Hosting.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Simple.Migrations.Tools.DotNet.Commands;
using Simple.Migrations.Tools.DotNet.Commands.Options;
using Simple.Migrations.Tools.DotNet.Migrations;
using Simple.Migrations.Tools.DotNet.Utilities;
using Simple.Migrations.Tools.DotNet.Utilities.IO;

namespace Simple.Migrations.Tools.DotNet
{
    public class Startup : IStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization();

            services.AddSingleton(PhysicalConsole.Singleton);
            services.AddSingleton<IFileSystem, PhysicalFileSystem>();

            services.AddScoped<CommandLineApplication, DatabaseCommand>()
                    .AddScoped<MigrateCommand>();

            services.AddScoped<CommandLineApplication, MigrationsCommand>()
                    .AddScoped<ListCommand>();

            services.AddCommonOptions(opts =>
                opts.Option("-c|--configuration")
                    .Option("-f|--framework")
                    .Option("-p|--project")
                    .Option("--no-build", CommandOptionType.NoValue)
                    .Option("--assembly")
                    .Option("--connection-string")
                    .Option("--connection-string-name"));

            services.AddSingleton<SimpleMigrations.ILogger, SimpleMigrations.Console.ConsoleLogger>();

            services.AddScoped<IMigratorFactory, MigratorFactory>();
            services.AddScoped<IProjectBuilder, ProjectBuilder>();
            services.AddScoped<IMigrationProjectInfoLocator, MigrationProjectInfoLocator>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCommands(c =>
            {
                c.Name = "sm";
                c.VersionOptionFromAssemblyAttributes(typeof(Startup).Assembly);
                c.Options.AddRange(app.ApplicationServices.GetService<ICommonOptions>());
            });
        }
    }
}
