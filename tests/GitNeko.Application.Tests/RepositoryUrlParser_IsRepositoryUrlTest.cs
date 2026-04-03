using GitNeko.Domain.Services;

namespace GitNeko.Application.Tests;

public class RepositoryUrlParser_IsRepositoryUrl
{
    [Fact]
    public void HTTPS_URLの場合_trueを返す()
    {
        var result = RepositoryUrlParser.IsRepositoryUrl("https://github.com/user/repo.git");

        Assert.True(result);
    }

    [Fact]
    public void HTTP_URLの場合_trueを返す()
    {
        var result = RepositoryUrlParser.IsRepositoryUrl("http://github.com/user/repo.git");

        Assert.True(result);
    }

    [Fact]
    public void SCP形式の場合_trueを返す()
    {
        var result = RepositoryUrlParser.IsRepositoryUrl("git@github.com:user/repo.git");

        Assert.True(result);
    }

    [Fact]
    public void 空文字列の場合_falseを返す()
    {
        var result = RepositoryUrlParser.IsRepositoryUrl("");

        Assert.False(result);
    }

    [Fact]
    public void 無関係なテキストの場合_falseを返す()
    {
        var result = RepositoryUrlParser.IsRepositoryUrl("hello world");

        Assert.False(result);
    }

    [Fact]
    public void nullの場合_falseを返す()
    {
        var result = RepositoryUrlParser.IsRepositoryUrl(null!);

        Assert.False(result);
    }
}
