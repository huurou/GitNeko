using GitNeko.Application.UseCases;
using GitNeko.Domain.Services;
using GitNeko.ViewModels;
using GitNeko.Views;
using System.Windows;

namespace GitNeko.Services;

public sealed class DialogService(
    CloneRepositoryUseCase cloneUseCase,
    IClipboardService clipboardService) : IDialogService
{
    public bool ShowCloneDialog(string parentDirectoryPath)
    {
        var viewModel = new CloneDialogViewModel(cloneUseCase, clipboardService, parentDirectoryPath);
        var dialog = new CloneDialog(viewModel)
        {
            Owner = System.Windows.Application.Current.MainWindow,
        };
        dialog.ShowDialog();
        return viewModel.IsCompleted;
    }
}
