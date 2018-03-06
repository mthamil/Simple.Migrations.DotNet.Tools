using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Simple.Migrations.Tools.DotNet.Utilities.IO;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Simple.Migrations.Tools.DotNet.Migrations
{
    public class MigrationProjectInfoLocator : IMigrationProjectInfoLocator
    {
        private readonly IConsole _console;
        private readonly IFileSystem _fileSystem;
        private readonly IProjectBuilder _projectBuilder;

        private readonly string _targetEnvironmentName;

        public MigrationProjectInfoLocator(IConsole console, IFileSystem fileSystem, IProjectBuilder projectBuilder)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _projectBuilder = projectBuilder ?? throw new ArgumentNullException(nameof(projectBuilder));

            _targetEnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        }

        public async Task<MigratorOptions> LocateAsync(
            bool shouldBuildIfNeeded,
            string configuration,
            string targetFramework,
            string project,
            string assembly,
            string connectionString,
            string connectionStringName)
        {
            if (assembly == null)
            {
                var projectFile = FindProjectFile(project);
                if (projectFile == null)
                    throw new ProjectNotFoundException("No assembly provided and project file could not be found.");

                using (var projectFileStream = projectFile.CreateReadStream())
                {
                    var projectDoc = await XDocument.LoadAsync(projectFileStream, LoadOptions.None, CancellationToken.None).ConfigureAwait(false);
                    var assemblyName = projectDoc.Root.Descendants("AssemblyName")
                                                      .Select(n => n.Value)
                                                      .SingleOrDefault() ?? Path.GetFileNameWithoutExtension(projectFile.Name);

                    targetFramework = FindTargetFramework(projectDoc, targetFramework);
                    configuration = configuration ?? "Debug";

                    if (shouldBuildIfNeeded)
                        await _projectBuilder.BuildAsync(projectFile, configuration, targetFramework).ConfigureAwait(false);

                    var projectDir = Path.GetDirectoryName(projectFile.PhysicalPath);
                    var assemblyFile = _fileSystem.GetFile(Path.Combine(projectDir, "bin", configuration, targetFramework, $"{assemblyName}.dll"));
                    if (!assemblyFile.Exists)
                        throw new ProjectNotFoundException("Assembly containing migrations could not be found.");

                    assembly = assemblyFile.PhysicalPath;
                }

                connectionString = connectionString ?? FindConnectionString(projectFile, connectionStringName);
            }

            return new MigratorOptions(
                Assembly.LoadFrom(assembly),
                connectionString);
        }

        private IFileInfo FindProjectFile(string initialProjectPath)
        {
            if (initialProjectPath == null)
                return _fileSystem.CurrentDirectory.SingleOrDefault(f => Path.GetExtension(f.Name).EndsWith("proj"));

            var projectDir = _fileSystem.GetDirectory(initialProjectPath);
            if (projectDir.Exists)
                return projectDir.SingleOrDefault(f => Path.GetExtension(f.Name).EndsWith("proj"));

            var projectFile = _fileSystem.GetFile(initialProjectPath);
            if (projectFile.Exists)
                return projectFile;

            return null;
        }

        private string FindTargetFramework(XDocument projectDoc, string providedFramework)
        {
            var projectFrameworks = projectDoc.Root.Descendants("TargetFramework")
                                                   .Concat(projectDoc.Root.Descendants("TargetFrameworks"))
                                                   .Select(t => t.Value)
                                                   .ToList();
            if (providedFramework == null)
            {
                if (projectFrameworks.Count == 0)
                    throw new TargetFrameworkNotFoundException("No target framework could be found.");

                if (projectFrameworks.Count > 1)
                    throw new TargetFrameworkNotFoundException("Multiple target frameworks available, please specifiy which to use with the --framework option.");
            }
            else
            {
                if (!projectFrameworks.Contains(providedFramework))
                    throw new TargetFrameworkNotFoundException($"Unsupported target framework. Available frameworks are: {String.Join(',', projectFrameworks)}.");
            }

            return providedFramework ?? projectFrameworks.Single();
        }

        private string FindConnectionString(IFileInfo projectFile, string connectionStringName)
        {
            var projectDir = Path.GetDirectoryName(projectFile.PhysicalPath);

            // Try to determine connection string from target project's configuration files.
            var targetConfig = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(projectDir, "appsettings.json"), true)
                .AddJsonFile(Path.Combine(projectDir, $"appsettings.{_targetEnvironmentName}.json"), true)
                .Build();

            var connectionStrings = targetConfig.GetSection("ConnectionStrings")
                                                .GetChildren()
                                                .ToList();
            if (connectionStrings.Count == 0)
                throw new ConnectionStringNotFoundException("No connection string could be found, please specify using the --connection-string option.");

            if (connectionStrings.Count > 1)
            {
                if (connectionStringName == null)
                    throw new ConnectionStringNotFoundException("Multiple connection strings available, please specify which to use with the --connection-string or --connection-string-name option.");

                var namedConnectionString = connectionStrings.SingleOrDefault(cs => cs.Key == connectionStringName);
                if (namedConnectionString == null)
                    throw new ConnectionStringNotFoundException($"Connection string named '{connectionStringName}' not found.");

                return namedConnectionString.Value;
            }

            return connectionStrings.Single().Value;
        }
    }
}
