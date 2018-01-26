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

            services.AddScoped<IMigratorFactory, MigratorFactory>();
        }

        public void Configure(IApplicationBuilder app)
        {
            var rootApp = app.ApplicationServices.GetService<RootCommandLineApplication>();
            using (var rootScope = app.ApplicationServices.CreateScope())
            {
                var commands = rootScope.ServiceProvider.GetServices<CommandLineApplication>();
                rootApp.Commands.AddRange(commands);
            }
        }
    }
}
