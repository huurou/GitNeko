using GitNeko.Domain.Services;

namespace GitNeko.Application.Tests;

public class RepositoryUrlParser_ExtractRepositoryName
{
    [Fact]
    public void HTTPS_dotgit付き_リポジトリ名を返す()
    {
        var result = RepositoryUrlParser.ExtractRepositoryName("https://github.com/user/repo.git");

        Assert.Equal("repo", result);
    }

    [Fact]
    public void HTTPS_dotgitなし_リポジトリ名を返す()
    {
        var result = RepositoryUrlParser.ExtractRepositoryName("https://github.com/user/repo");

        Assert.Equal("repo", result);
    }

    [Fact]
    public void SCP形式_dotgit付き_リポジトリ名を返す()
    {
        var result = RepositoryUrlParser.ExtractRepositoryName("git@github.com:user/repo.git");

        Assert.Equal("repo", result);
    }

    [Fact]
    public void SCP形式_dotgitなし_リポジトリ名を返す()
    {
        var result = RepositoryUrlParser.ExtractRepositoryName("git@github.com:user/repo");

        Assert.Equal("repo", result);
    }

    [Fact]
    public void 末尾スラッシュ付き_リポジトリ名を返す()
    {
        var result = RepositoryUrlParser.ExtractRepositoryName("https://github.com/user/repo/");

        Assert.Equal("repo", result);
    }

    [Fact]
    public void 空文字列_空文字列を返す()
    {
        var result = RepositoryUrlParser.ExtractRepositoryName("");

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void null_空文字列を返す()
    {
        var result = RepositoryUrlParser.ExtractRepositoryName(null!);

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void 無効な文字列_空文字列を返す()
    {
        var result = RepositoryUrlParser.ExtractRepositoryName("invalid");

        Assert.Equal(string.Empty, result);
    }
}
