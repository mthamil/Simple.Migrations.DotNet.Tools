using System;

namespace Simple.Migrations.Tools.DotNet.Commands.Options
{
    public class CommandOptionException : Exception
    {
        public CommandOptionException(string longName, string message)
            : base(message)
        {
            LongName = longName;
        }

        public string LongName { get; }
    }
}
