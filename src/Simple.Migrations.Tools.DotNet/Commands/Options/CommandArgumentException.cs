using System;

namespace Simple.Migrations.Tools.DotNet.Commands.Options
{

    public class CommandArgumentException : Exception
    {
        public CommandArgumentException(string name, string message)
            : base(message)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
