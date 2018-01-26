using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.DependencyModel.Resolution;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Simple.Migrations.Tools.DotNet.Utilities
{
    public class DependencyAwareAssemblyLoader : IDisposable
    {
        private readonly ICompilationAssemblyResolver _resolver;
        private readonly DependencyContext _dependencyContext;
        private readonly AssemblyLoadContext _assemblyLoadContext;

        public DependencyAwareAssemblyLoader(Assembly root)
        {
            Assembly = root ?? throw new ArgumentNullException(nameof(root));

            _resolver = new CompositeCompilationAssemblyResolver(new ICompilationAssemblyResolver[]
            {
                new AppBaseCompilationAssemblyResolver(Path.GetDirectoryName(root.Location)),
                new ReferenceAssemblyPathResolver(),
                new PackageCompilationAssemblyResolver()
            });

            _dependencyContext = DependencyContext.Load(root);

            _assemblyLoadContext = AssemblyLoadContext.GetLoadContext(root);
            _assemblyLoadContext.Resolving += Resolve;

        }

        public Assembly Assembly { get; }

        private Assembly Resolve(AssemblyLoadContext context, AssemblyName assemblyName)
        {
            var dependency = _dependencyContext.RuntimeLibraries.SingleOrDefault(d => d.Name.Equals(assemblyName.Name, StringComparison.OrdinalIgnoreCase));
            if (dependency == null)
                return null;

            var library = new CompilationLibrary(
                dependency.Type,
                dependency.Name,
                dependency.Version,
                dependency.Hash,
                dependency.RuntimeAssemblyGroups.SelectMany(g => g.AssetPaths),
                dependency.Dependencies,
                dependency.Serviceable);

            var assemblies = new List<string>();
            if (_resolver.TryResolveAssemblyPaths(library, assemblies) && assemblies.Count > 0)
            {
                try
                {
                    return _assemblyLoadContext.LoadFromAssemblyPath(assemblies[0]);
                }
                catch (Exception e)
                {
                    Debug.Write(e);
                }
            }
            
            return null;
        }

        public void Dispose()
        {
            _assemblyLoadContext.Resolving -= Resolve;
        }
    }
}
