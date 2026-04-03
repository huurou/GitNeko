namespace GitNeko.Domain.Repositories;

public interface IGitCloneService
{
    Task CloneAsync(
        CloneRequest request,
        IProgress<CloneProgress>? progress = null,
        CancellationToken cancellationToken = default);
}
