using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Simple.Migrations.Tools.DotNet.Utilities.Diagnostics
{
    public static class ProcessExtensions
    {
        public static async Task StartAsync(this Process process,
                                            Func<StreamReader, Task> readOutput,
                                            Func<StreamReader, Task> readError)
        {
            if (process.Start())
            {
                var readErrorTask = readError(process.StandardError);
                var readOutputTask = readOutput(process.StandardOutput);

                await Task.WhenAll(readErrorTask, readOutputTask).ConfigureAwait(false);
                process.StandardOutput.Dispose();
                process.StandardError.Dispose();

                await Task.Run(() => process.WaitForExit()).ConfigureAwait(false);
            }
        }
    }
}
