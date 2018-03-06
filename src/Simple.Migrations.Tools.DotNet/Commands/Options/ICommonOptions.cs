using McMaster.Extensions.CommandLineUtils;
using System.Collections.Generic;

namespace Simple.Migrations.Tools.DotNet.Commands.Options
{
    public interface ICommonOptions : IEnumerable<CommandOption>
    {
        CommandOption this[string longName] { get; }
    }
}
