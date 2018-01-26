using McMaster.Extensions.CommandLineUtils;
using System;

namespace Simple.Migrations.Tools.DotNet.Utilities
{
    public static class CommandOptionExtensions
    {
        public static bool IsOn(this CommandOption option)
        {
            if (option.OptionType != CommandOptionType.NoValue)
                throw new InvalidOperationException($"Option must have type '{CommandOptionType.NoValue:G}'.");

            return option.Value() == "on";
        }
    }
}
