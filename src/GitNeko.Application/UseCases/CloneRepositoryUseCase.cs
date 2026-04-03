using GitNeko.Domain.Repositories;

namespace GitNeko.Application.UseCases;

public sealed class CloneRepositoryUseCase(IGitCloneService cloneService)
{
    public Task ExecuteAsync(
        CloneRequest request,
        IProgress<CloneProgress>? progress = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(request.RepositoryUrl);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.ParentDirectoryPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.FolderName);

        return cloneService.CloneAsync(request, progress, cancellationToken);
    }
}
