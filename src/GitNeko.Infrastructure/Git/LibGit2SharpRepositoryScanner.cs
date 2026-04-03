using System.IO;
using System.Security;
using GitNeko.Domain.Repositories;
using GitNeko.Domain.Services;
using LibGit2Sharp;

namespace GitNeko.Infrastructure.Git;

public sealed class LibGit2SharpRepositoryScanner : IGitRepositoryScanner
{
    public IReadOnlyList<GitRepository> ScanDirectory(string directoryPath)
    {
        string[] subDirs;
        try { subDirs = Directory.GetDirectories(directoryPath); }
        catch (UnauthorizedAccessException) { return []; }
        catch (SecurityException) { return []; }

        var results = new List<GitRepository>();

        foreach (var subDir in subDirs)
        {
            try
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
            catch (UnauthorizedAccessException)
            {
                continue;
            }
            catch (SecurityException)
            {
                continue;
            }
        }

        return results;
    }
}
