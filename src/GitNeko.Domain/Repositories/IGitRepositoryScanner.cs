namespace GitNeko.Domain.Repositories;

public interface IGitRepositoryScanner
{
    IReadOnlyList<GitRepository> ScanDirectory(string directoryPath);
}
