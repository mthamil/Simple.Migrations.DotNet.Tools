using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.FileProviders;
using Simple.Migrations.Tools.DotNet.Utilities.Diagnostics;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Simple.Migrations.Tools.DotNet.Migrations
{
    public class ProjectBuilder : IProjectBuilder
    {
        private readonly IConsole _console;

        public ProjectBuilder(IConsole console)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
        }

        public async Task BuildAsync(IFileInfo project, string configuration, string framework)
        {
            _console.WriteLine("Building project...");
            var processInfo = new ProcessStartInfo(DotNetExe.FullPathOrDefault())
            {
                Arguments = $"build {project.Name} --configuration {configuration} --framework {framework}",
                WorkingDirectory = Path.GetDirectoryName(project.PhysicalPath),
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            using (var process = new Process { StartInfo = processInfo })
            {
                await process.StartAsync(
                    async output =>
                    {
                        while (!output.EndOfStream)
                            await _console.Out.WriteLineAsync(
                                await output.ReadLineAsync().ConfigureAwait(false)).ConfigureAwait(false);
                    },
                    async error =>
                    {
                        while (!error.EndOfStream)
                            await _console.Error.WriteLineAsync(
                                await error.ReadLineAsync().ConfigureAwait(false)).ConfigureAwait(false);
                    }).ConfigureAwait(false);
            }

            _console.WriteLine("Build completed.")
                    .WriteLine(string.Empty);
        }
    }
}
