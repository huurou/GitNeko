using GitNeko.Application.UseCases;
using GitNeko.Domain.Repositories;
using GitNeko.Domain.Services;

namespace GitNeko.Application.Tests;

public class CloneRepositoryUseCaseTest
{
    [Fact]
    public async Task クローン_URL空_例外を投げる()
    {
        var service = new FakeCloneService();
        var useCase = new CloneRepositoryUseCase(service);
        var request = new CloneRequest("", @"C:\test", "repo");

        await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync(request, cancellationToken: TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task クローン_フォルダ名空_例外を投げる()
    {
        var service = new FakeCloneService();
        var useCase = new CloneRepositoryUseCase(service);
        var request = new CloneRequest("https://example.com/repo.git", @"C:\test", "");

        await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync(request, cancellationToken: TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task クローン_有効なリクエスト_サービスが呼ばれる()
    {
        var service = new FakeCloneService();
        var useCase = new CloneRepositoryUseCase(service);
        var request = new CloneRequest("https://example.com/repo.git", @"C:\test", "repo");

        await useCase.ExecuteAsync(request, cancellationToken: TestContext.Current.CancellationToken);

        Assert.True(service.WasCalled);
    }

    [Fact]
    public async Task クローン_相対パス_例外を投げる()
    {
        var service = new FakeCloneService();
        var useCase = new CloneRepositoryUseCase(service);
        var request = new CloneRequest("https://example.com/repo.git", "relative/path", "repo");

        await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync(request, cancellationToken: TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task クローン_Windows絶対パス_サービスが呼ばれる()
    {
        var service = new FakeCloneService();
        var useCase = new CloneRepositoryUseCase(service);
        var request = new CloneRequest("https://example.com/repo.git", @"D:\repos", "repo");

        await useCase.ExecuteAsync(request, cancellationToken: TestContext.Current.CancellationToken);

        Assert.True(service.WasCalled);
    }

    [Fact]
    public async Task クローン_Unix絶対パス_サービスが呼ばれる()
    {
        var service = new FakeCloneService();
        var useCase = new CloneRepositoryUseCase(service);
        var request = new CloneRequest("https://example.com/repo.git", "/home/user/repos", "repo");

        await useCase.ExecuteAsync(request, cancellationToken: TestContext.Current.CancellationToken);

        Assert.True(service.WasCalled);
    }

    [Fact]
    public async Task クローン_ローカルWindows絶対パス_サービスが呼ばれる()
    {
        var service = new FakeCloneService();
        var useCase = new CloneRepositoryUseCase(service);
        var request = new CloneRequest(@"C:\repos\source.git", @"C:\test", "repo");

        await useCase.ExecuteAsync(request, cancellationToken: TestContext.Current.CancellationToken);

        Assert.True(service.WasCalled);
    }

    [Fact]
    public async Task クローン_ローカルUnix絶対パス_サービスが呼ばれる()
    {
        var service = new FakeCloneService();
        var useCase = new CloneRepositoryUseCase(service);
        var request = new CloneRequest("/home/user/example.git", @"C:\test", "repo");

        await useCase.ExecuteAsync(request, cancellationToken: TestContext.Current.CancellationToken);

        Assert.True(service.WasCalled);
    }

    [Fact]
    public async Task クローン_ローカル相対パス_例外を投げる()
    {
        var service = new FakeCloneService();
        var useCase = new CloneRepositoryUseCase(service);
        var request = new CloneRequest("relative/path/repo", @"C:\test", "repo");

        await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync(request, cancellationToken: TestContext.Current.CancellationToken));
    }

    private sealed class FakeCloneService : IGitCloneService
    {
        public bool WasCalled { get; private set; }

        public Task CloneAsync(CloneRequest request, IProgress<CloneProgress>? progress = null, CancellationToken cancellationToken = default)
        {
            WasCalled = true;
            return Task.CompletedTask;
        }
    }
}
