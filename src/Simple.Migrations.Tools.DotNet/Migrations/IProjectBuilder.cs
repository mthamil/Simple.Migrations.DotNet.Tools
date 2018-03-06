using Microsoft.Extensions.FileProviders;
using System.Threading.Tasks;

namespace Simple.Migrations.Tools.DotNet.Migrations
{
    public interface IProjectBuilder
    {
        Task BuildAsync(IFileInfo project, string configuration, string framework);
    }
}