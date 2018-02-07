using McMaster.Extensions.CommandLineUtils;
using Simple.Migrations.Tools.DotNet.Migrations;
using System;
namespace Simple.Migrations.Tools.DotNet
{
    public class MigrationsCommand : CommandLineApplication
    {
        public MigrationsCommand(ListCommand listCommand)
        {
            Name = "migrations";
            Description = "Migrates a database.";
            Commands.Add(listCommand);
        }
    }
}
