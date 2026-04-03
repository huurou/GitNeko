using GitNeko.Application.UseCases;
using GitNeko.Domain.Repositories;

namespace GitNeko.Domain.Tests;

public class CloneRepositoryUseCaseTest
{
    [Fact]
    public async Task クローン_URL空_例外を投げる()
    {
        var service = new FakeCloneService();
        var useCase = new CloneRepositoryUseCase(service);
        var request = new CloneRequest("", @"C:\test", "repo");

        await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync(request));
    }

    [Fact]
    public async Task クローン_フォルダ名空_例外を投げる()
    {
        var service = new FakeCloneService();
        var useCase = new CloneRepositoryUseCase(service);
        var request = new CloneRequest("https://example.com/repo.git", @"C:\test", "");

        await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync(request));
    }

    [Fact]
    public async Task クローン_有効なリクエスト_サービスが呼ばれる()
    {
        var service = new FakeCloneService();
        var useCase = new CloneRepositoryUseCase(service);
        var request = new CloneRequest("https://example.com/repo.git", @"C:\test", "repo");

        await useCase.ExecuteAsync(request);

        Assert.True(service.WasCalled);
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
