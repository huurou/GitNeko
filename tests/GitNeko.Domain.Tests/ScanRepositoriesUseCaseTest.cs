using GitNeko.Application.UseCases;
using GitNeko.Domain.Repositories;

namespace GitNeko.Domain.Tests;

public class ScanRepositoriesUseCaseTest
{
    [Fact]
    public void スキャン_存在しないフォルダ_空リストを返す()
    {
        var scanner = new FakeScanner([]);
        var useCase = new ScanRepositoriesUseCase(scanner);

        var result = useCase.Execute(@"C:\存在しないフォルダ");

        Assert.Empty(result);
    }

    [Fact]
    public void スキャン_リポジトリがある_リストを返す()
    {
        var repos = new List<GitRepository>
        {
            new() { Name = "repo1", FullPath = @"C:\test\repo1", CurrentBranch = "main" },
            new() { Name = "repo2", FullPath = @"C:\test\repo2", CurrentBranch = "develop" },
        };
        var scanner = new FakeScanner(repos);
        var tempDir = Path.GetTempPath();
        var useCase = new ScanRepositoriesUseCase(scanner);

        var result = useCase.Execute(tempDir);

        Assert.Equal(2, result.Count);
        Assert.Equal("repo1", result[0].Name);
    }

    private sealed class FakeScanner(IReadOnlyList<GitRepository> repositories) : IGitRepositoryScanner
    {
        public IReadOnlyList<GitRepository> ScanDirectory(string directoryPath) => repositories;
    }
}
