using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Simple.Migrations.Tools.DotNet.Commands.Options;
using System;

namespace Simple.Migrations.Tools.DotNet.Utilities
{
    public static class CommonOptionServiceCollectionExtensions
    {
        public static IServiceCollection AddCommonOptions(this IServiceCollection services, Action<ICommonOptionsBuilder> optionsBuilder) =>
            services.AddSingleton<ICommonOptions>(provider =>
            {
                var options = new CommonOptions(provider.GetService<IStringLocalizerFactory>());
                optionsBuilder(options);
                return options;
            });
    }
}
