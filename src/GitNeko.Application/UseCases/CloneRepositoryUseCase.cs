using GitNeko.Domain.Repositories;
using GitNeko.Domain.Services;

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
        if (!Path.IsPathRooted(request.ParentDirectoryPath))
            throw new ArgumentException("絶対パスを指定してください。", nameof(request));
        ArgumentException.ThrowIfNullOrWhiteSpace(request.FolderName);

        return cloneService.CloneAsync(request, progress, cancellationToken);
    }
}
