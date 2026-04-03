namespace GitNeko.Domain.Repositories;

public sealed record CloneProgress(string Message, int? PercentComplete);
