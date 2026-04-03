using System.IO;
using GitNeko.Domain.Repositories;
using LibGit2Sharp;

namespace GitNeko.Infrastructure.Git;

public sealed class LibGit2SharpCloneService : IGitCloneService
{
    public Task CloneAsync(
        CloneRequest request,
        IProgress<CloneProgress>? progress = null,
        CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            var targetPath = Path.Combine(request.ParentDirectoryPath, request.FolderName);
            var fetchOptions = new FetchOptions();

            if (progress is not null)
            {
                fetchOptions.OnTransferProgress = transferProgress =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var percent = transferProgress.TotalObjects > 0
                        ? (int)((double)transferProgress.ReceivedObjects / transferProgress.TotalObjects * 100)
                        : 0;
                    progress.Report(new CloneProgress(
                        $"オブジェクト取得中... {transferProgress.ReceivedObjects}/{transferProgress.TotalObjects}",
                        percent));
                    return true;
                };
            }

            var options = new CloneOptions(fetchOptions);
            Repository.Clone(request.RepositoryUrl, targetPath, options);
            progress?.Report(new CloneProgress("クローン完了", 100));
        }, cancellationToken);
    }
}
