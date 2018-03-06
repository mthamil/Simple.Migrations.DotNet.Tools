using System;

namespace Simple.Migrations.Tools.DotNet.Migrations
{
    public class ProjectNotFoundException : Exception
    {
        public ProjectNotFoundException(string message) : base(message) { }
    }
}
