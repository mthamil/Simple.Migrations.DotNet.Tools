using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simple.Migrations.Tools.DotNet.Utilities
{
    public static class CommandLineApplicationExtensions
    {
        public static Lazy<CommandOption> InheritedOption(this CommandLineApplication app, string longName) =>
            new Lazy<CommandOption>(() => app.AllCommands().FirstOrDefault(c => c.LongName == longName));

        public static IEnumerable<CommandOption> AllCommands(this CommandLineApplication app)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            IEnumerable<CommandOption> CollectOptions(CommandLineApplication cmdApp) =>
                cmdApp == null
                    ? Enumerable.Empty<CommandOption>() 
                    : cmdApp.Options.Concat(CollectOptions(cmdApp.Parent));

            return CollectOptions(app);
        }
    }
}
