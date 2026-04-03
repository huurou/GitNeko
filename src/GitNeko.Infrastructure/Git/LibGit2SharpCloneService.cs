using System.IO;
using GitNeko.Domain.Repositories;
using GitNeko.Domain.Services;
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
            var targetExistedBefore = Directory.Exists(targetPath);
            var fetchOptions = new FetchOptions
            {
                OnTransferProgress = transferProgress =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    if (progress is not null)
                    {
                        var percent = transferProgress.TotalObjects > 0
                            ? (int)((double)transferProgress.ReceivedObjects / transferProgress.TotalObjects * 100)
                            : 0;
                        progress.Report(new CloneProgress(
                            $"オブジェクト取得中... {transferProgress.ReceivedObjects}/{transferProgress.TotalObjects}",
                            percent));
                    }
                    return true;
                },
            };

            var options = new CloneOptions(fetchOptions)
            {
                OnCheckoutProgress = (path, completedSteps, totalSteps) =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                },
            };

            try
            {
                Repository.Clone(request.RepositoryUrl, targetPath, options);
                progress?.Report(new CloneProgress("クローン完了", 100));
            }
            catch
            {
                if (!targetExistedBefore && Directory.Exists(targetPath))
                {
                    try { Directory.Delete(targetPath, true); }
                    catch { /* クリーンアップ失敗は元例外を優先 */ }
                }
                throw;
            }
        }, cancellationToken);
    }
}
