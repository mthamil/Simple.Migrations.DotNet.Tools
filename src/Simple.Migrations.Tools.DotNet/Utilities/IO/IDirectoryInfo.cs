using Microsoft.Extensions.FileProviders;

namespace Simple.Migrations.Tools.DotNet.Utilities.IO
{

    public interface IDirectoryInfo : IFileInfo, IDirectoryContents
    {
        new bool Exists { get; }
    }
}