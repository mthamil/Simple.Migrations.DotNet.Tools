using CommandLine.Core.Hosting.Abstractions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Internal;
using Microsoft.Extensions.FileProviders.Physical;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Simple.Migrations.Tools.DotNet.Utilities.IO
{
    class PhysicalFileSystem : IFileSystem
    {
        public PhysicalFileSystem(IHostingEnvironment hostingEnvironment)
        {
            CurrentDirectory = new PhysicalDirectory(hostingEnvironment.WorkingDirectory);
        }

        public IDirectoryInfo CurrentDirectory { get; }

        public IDirectoryInfo GetDirectory(string path) => new PhysicalDirectory(path);

        public IFileInfo GetFile(string path) => new PhysicalFileInfo(new FileInfo(path));

        class PhysicalDirectory : IDirectoryInfo
        {
            private readonly PhysicalDirectoryInfo _directoryInfo;
            private readonly PhysicalDirectoryContents _contents;

            public PhysicalDirectory(string path)
            {
                _directoryInfo = new PhysicalDirectoryInfo(new DirectoryInfo(path));
                _contents = new PhysicalDirectoryContents(path);
            }

            public bool Exists => _directoryInfo.Exists;

            public long Length => _directoryInfo.Length;

            public string PhysicalPath => _directoryInfo.PhysicalPath;

            public string Name => _directoryInfo.Name;

            public DateTimeOffset LastModified => _directoryInfo.LastModified;

            public bool IsDirectory => _directoryInfo.IsDirectory;

            public Stream CreateReadStream() => _directoryInfo.CreateReadStream();

            public IEnumerator<IFileInfo> GetEnumerator() => _contents.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => _contents.GetEnumerator();
        }
    }
}
