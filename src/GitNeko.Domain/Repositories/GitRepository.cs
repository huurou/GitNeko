namespace GitNeko.Domain.Repositories;

public sealed class GitRepository
{
    public required string Name { get; init; }
    public required string FullPath { get; init; }
    public string? CurrentBranch { get; init; }
}
