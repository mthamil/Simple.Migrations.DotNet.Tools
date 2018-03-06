using System;

namespace Simple.Migrations.Tools.DotNet.Migrations
{
    public class ConnectionStringNotFoundException : Exception
    {
        public ConnectionStringNotFoundException(string message) : base(message) { }
    }
}
