using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GitNeko.Application.UseCases;
using GitNeko.Domain.Repositories;
using GitNeko.Domain.Services;
using GitNeko.Services;

namespace GitNeko.ViewModels;

public sealed partial class CloneDialogViewModel : ObservableObject
{
    private readonly CloneRepositoryUseCase _cloneUseCase;
    private readonly string _parentDirectoryPath;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ExecuteCloneCommand))]
    private string _repositoryUrl = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ExecuteCloneCommand))]
    private string _folderName = string.Empty;

    [ObservableProperty]
    private string _progressMessage = string.Empty;

    [ObservableProperty]
    private int _progressPercent;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ExecuteCloneCommand))]
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
        if (!string.IsNullOrWhiteSpace(clipboardText) && RepositoryUrlParser.IsRepositoryUrl(clipboardText))
            RepositoryUrl = clipboardText;
    }

    partial void OnRepositoryUrlChanged(string value)
    {
        var repoName = RepositoryUrlParser.ExtractRepositoryName(value);
        if (!string.IsNullOrEmpty(repoName))
            FolderName = repoName;
    }

    private bool CanExecuteClone() =>
        !string.IsNullOrWhiteSpace(RepositoryUrl) && !string.IsNullOrWhiteSpace(FolderName) && !IsCloning;

    [RelayCommand(CanExecute = nameof(CanExecuteClone), IncludeCancelCommand = true)]
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
}
