using McMaster.Extensions.CommandLineUtils;

namespace Simple.Migrations.Tools.DotNet.Commands.Options
{
    public interface ICommonOptionsBuilder
    {
        ICommonOptionsBuilder Option(string template, CommandOptionType optionType = CommandOptionType.SingleValue);
    }
}
