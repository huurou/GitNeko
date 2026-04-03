using System.Text.RegularExpressions;

namespace GitNeko.Domain.Services;

public static partial class RepositoryUrlParser
{
    public static bool IsRepositoryUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return false;

        if (url.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ||
            url.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
            return true;

        if (ScpPattern().IsMatch(url))
            return true;

        return false;
    }

    public static string ExtractRepositoryName(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return string.Empty;

        var trimmed = url.TrimEnd('/');
        if (trimmed.EndsWith(".git", StringComparison.OrdinalIgnoreCase))
            trimmed = trimmed[..^4];

        var scpMatch = ScpPattern().Match(trimmed);
        if (scpMatch.Success)
        {
            var path = scpMatch.Groups[1].Value;
            var lastSlash = path.LastIndexOf('/');
            return lastSlash >= 0 ? path[(lastSlash + 1)..] : path;
        }

        var lastSep = trimmed.LastIndexOf('/');
        return lastSep >= 0 ? trimmed[(lastSep + 1)..] : string.Empty;
    }

    [GeneratedRegex(@"^[\w.-]+@[\w.-]+:(.+)$")]
    private static partial Regex ScpPattern();
}
