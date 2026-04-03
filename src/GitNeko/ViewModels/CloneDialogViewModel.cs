using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GitNeko.Application.UseCases;
using GitNeko.Domain.Repositories;
using GitNeko.Domain.Services;

namespace GitNeko.ViewModels;

public sealed partial class CloneDialogViewModel : ObservableObject
{
    private readonly CloneRepositoryUseCase _cloneUseCase;
    private readonly string _parentDirectoryPath;

    [ObservableProperty]
    private string _repositoryUrl = string.Empty;

    [ObservableProperty]
    private string _folderName = string.Empty;

    [ObservableProperty]
    private string _progressMessage = string.Empty;

    [ObservableProperty]
    private int _progressPercent;

    [ObservableProperty]
    private bool _isCloning;

    public bool IsCompleted { get; private set; }

    public CloneDialogViewModel(
        CloneRepositoryUseCase cloneUseCase,
        IClipboardService clipboardService,
        string parentDirectoryPath)
    {
        _cloneUseCase = cloneUseCase;
        _parentDirectoryPath = parentDirectoryPath;

        var clipboardText = clipboardService.GetText();
        if (!string.IsNullOrWhiteSpace(clipboardText))
            RepositoryUrl = clipboardText;
    }

    partial void OnRepositoryUrlChanged(string value)
    {
        var repoName = ExtractRepositoryName(value);
        if (!string.IsNullOrEmpty(repoName))
            FolderName = repoName;
    }

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task ExecuteCloneAsync(CancellationToken cancellationToken)
    {
        IsCloning = true;
        ProgressMessage = "クローンを開始します...";
        ProgressPercent = 0;

        try
        {
            var request = new CloneRequest(RepositoryUrl, _parentDirectoryPath, FolderName);
            var progress = new Progress<CloneProgress>(p =>
            {
                ProgressMessage = p.Message;
                ProgressPercent = p.PercentComplete ?? 0;
            });

            await _cloneUseCase.ExecuteAsync(request, progress, cancellationToken);
            ProgressMessage = "クローンが完了しました。";
            IsCompleted = true;
        }
        catch (OperationCanceledException)
        {
            ProgressMessage = "クローンがキャンセルされました。";
        }
        catch (Exception ex)
        {
            ProgressMessage = $"エラー: {ex.Message}";
        }
        finally
        {
            IsCloning = false;
        }
    }

    internal static string ExtractRepositoryName(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return string.Empty;

        var trimmed = url.TrimEnd('/');
        if (trimmed.EndsWith(".git", StringComparison.OrdinalIgnoreCase))
            trimmed = trimmed[..^4];

        var lastSlash = trimmed.LastIndexOf('/');
        return lastSlash >= 0 ? trimmed[(lastSlash + 1)..] : string.Empty;
    }
}
