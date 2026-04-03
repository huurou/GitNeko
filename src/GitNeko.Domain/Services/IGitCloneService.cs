using GitNeko.Domain.Repositories;

namespace GitNeko.Domain.Services;

public interface IGitCloneService
{
    Task CloneAsync(
        CloneRequest request,
        IProgress<CloneProgress>? progress = null,
        CancellationToken cancellationToken = default);
}
