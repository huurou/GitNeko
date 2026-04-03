using System.IO;
using GitNeko.Domain.Repositories;
using LibGit2Sharp;

namespace GitNeko.Infrastructure.Git;

public sealed class LibGit2SharpRepositoryScanner : IGitRepositoryScanner
{
    public IReadOnlyList<GitRepository> ScanDirectory(string directoryPath)
    {
        var results = new List<GitRepository>();

        foreach (var subDir in Directory.GetDirectories(directoryPath))
        {
            if (!Repository.IsValid(subDir))
                continue;

            using var repo = new Repository(subDir);
            results.Add(new GitRepository
            {
                Name = Path.GetFileName(subDir),
                FullPath = subDir,
                CurrentBranch = repo.Head?.FriendlyName,
            });
        }

        return results;
    }
}
