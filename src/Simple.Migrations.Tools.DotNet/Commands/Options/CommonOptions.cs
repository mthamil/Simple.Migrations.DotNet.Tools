using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Localization;
using Simple.Migrations.Tools.DotNet.Resources;
using Simple.Migrations.Tools.DotNet.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Simple.Migrations.Tools.DotNet.Commands.Options
{
    public class CommonOptions : ICommonOptions, ICommonOptionsBuilder
    {
        private readonly IStringLocalizer _descriptions;

        private readonly IList<CommandOption> _options = new List<CommandOption>();

        public CommonOptions(IStringLocalizerFactory stringLocalizerFactory)
        {
            _descriptions = stringLocalizerFactory.Create(typeof(OptionDescriptions));
        }

        public CommandOption this[string longName] => _options.SingleOrDefault(o => o.LongName == longName);

        public ICommonOptionsBuilder Option(string template, CommandOptionType optionType)
        {
            var option = new CommandOption(template, optionType) { Inherited = true };
            option.Description = _descriptions[CreateResourceKey(option.LongName)];
            _options.Add(option);
            return this;
        }

        private static string CreateResourceKey(string longName) => longName.ToPascalCase();

        public IEnumerator<CommandOption> GetEnumerator() => _options.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
