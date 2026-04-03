using GitNeko.Domain.Repositories;

namespace GitNeko.Application.UseCases;

public sealed class ScanRepositoriesUseCase(IGitRepositoryScanner scanner)
{
    public IReadOnlyList<GitRepository> Execute(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
            return [];

        return scanner.ScanDirectory(directoryPath);
    }
}
