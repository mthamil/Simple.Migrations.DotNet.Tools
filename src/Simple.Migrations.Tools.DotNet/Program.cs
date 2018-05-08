using CommandLine.Core.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Simple.Migrations.Tools.DotNet
{
    class Program
    {
        public static Task<int> Main(string[] args) =>
            CommandLineHost.CreateBuilder(args)
                           .ConfigureLogging(l => l.AddConsole())
                           .UseStartup<Startup>()
                           .Build()
                           .RunAsync();
    }
}
