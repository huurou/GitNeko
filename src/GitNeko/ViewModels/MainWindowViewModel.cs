using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GitNeko.Application.UseCases;
using GitNeko.Domain.Repositories;
using GitNeko.Services;
using System.Collections.ObjectModel;

namespace GitNeko.ViewModels;

public sealed partial class MainWindowViewModel : ObservableObject
{
    private readonly ScanRepositoriesUseCase _scanUseCase;
    private readonly IDialogService _dialogService;

    [ObservableProperty]
    private string _folderPath = string.Empty;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    public ObservableCollection<GitRepository> Repositories { get; } = [];

    public MainWindowViewModel(
        ScanRepositoriesUseCase scanUseCase,
        IDialogService dialogService)
    {
        _scanUseCase = scanUseCase;
        _dialogService = dialogService;
    }

    [RelayCommand]
    private void ScanFolder()
    {
        Repositories.Clear();

        if (string.IsNullOrWhiteSpace(FolderPath))
        {
            StatusMessage = "フォルダパスを入力してください。";
            return;
        }

        var repos = _scanUseCase.Execute(FolderPath);
        foreach (var repo in repos)
            Repositories.Add(repo);

        StatusMessage = repos.Count > 0
            ? $"{repos.Count} 件のリポジトリが見つかりました。"
            : "リポジトリが見つかりませんでした。";
    }

    [RelayCommand]
    private void OpenCloneDialog()
    {
        if (string.IsNullOrWhiteSpace(FolderPath))
        {
            StatusMessage = "フォルダパスを入力してください。";
            return;
        }

        var cloned = _dialogService.ShowCloneDialog(FolderPath);
        if (cloned)
            ScanFolder();
    }
}
