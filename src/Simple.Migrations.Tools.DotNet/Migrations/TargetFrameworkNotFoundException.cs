using System;

namespace Simple.Migrations.Tools.DotNet.Migrations
{
    public class TargetFrameworkNotFoundException : Exception
    {
        public TargetFrameworkNotFoundException(string message) : base(message) { }
    }
}
