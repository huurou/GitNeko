namespace GitNeko.Domain.Repositories;

public sealed record CloneRequest(
    string RepositoryUrl,
    string ParentDirectoryPath,
    string FolderName);
