using GitNeko.Domain.Repositories;

namespace GitNeko.Domain.Services;

public interface IGitRepositoryScanner
{
    IReadOnlyList<GitRepository> ScanDirectory(string directoryPath);
}
