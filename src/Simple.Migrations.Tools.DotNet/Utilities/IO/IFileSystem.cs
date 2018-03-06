using Microsoft.Extensions.FileProviders;

namespace Simple.Migrations.Tools.DotNet.Utilities.IO
{
    public interface IFileSystem
    {
        IDirectoryInfo CurrentDirectory { get; }

        IDirectoryInfo GetDirectory(string path);

        IFileInfo GetFile(string path);
    }
}