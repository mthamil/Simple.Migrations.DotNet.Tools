using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using CommandLine.Core.Hosting;
using Simple.Migrations.Tools.DotNet.Commands;
using Simple.Migrations.Tools.DotNet.Migrations;
using CommandLine.Core.Hosting.CommandLineUtils;

namespace Simple.Migrations.Tools.DotNet
{
    public class Startup : IStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(PhysicalConsole.Singleton);

            services.AddScoped<CommandLineApplication, DatabaseCommand>();
            services.AddScoped<MigrateCommand>();

            services.AddScoped<CommandLineApplication, MigrationsCommand>();
            services.AddScoped<ListCommand>();

            services.AddScoped<IMigratorFactory, MigratorFactory>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCommands(c =>
            {
                c.Name = "sm";
                c.VersionOptionFromAssemblyAttributes(typeof(Startup).Assembly);
            });
        }
    }
}
