using CommandLine.Core.Hosting;
using CommandLine.Core.Hosting.CommandLineUtils;
using System.Threading.Tasks;

namespace Simple.Migrations.Tools.DotNet
{
    class Program
    {
        public static Task<int> Main(string[] args) =>
            CommandLineHost.CreateBuilder(args)
                           .UseCommandLineUtils()
                           .UseStartup<Startup>()
                           .Build()
                           .RunAsync();
    }
}
