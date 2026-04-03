using GitNeko.Domain.Repositories;
using GitNeko.Domain.Services;
using GitNeko.Infrastructure.Clipboard;
using GitNeko.Infrastructure.Git;
using Microsoft.Extensions.DependencyInjection;

namespace GitNeko.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IGitRepositoryScanner, LibGit2SharpRepositoryScanner>();
        services.AddSingleton<IGitCloneService, LibGit2SharpCloneService>();
        services.AddSingleton<IClipboardService, ClipboardService>();
        return services;
    }
}
