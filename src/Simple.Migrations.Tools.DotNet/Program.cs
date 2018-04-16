using CommandLine.Core.Hosting;
using CommandLine.Core.CommandLineUtils;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Simple.Migrations.Tools.DotNet
{
    class Program
    {
        public static Task<int> Main(string[] args) =>
            CommandLineHost.CreateBuilder(args)
                           .ConfigureLogging(l => l.AddConsole())
                           .UseCommandLineUtils()
                           .UseStartup<Startup>()
                           .Build()
                           .RunAsync();
    }
}
