using GitNeko.ViewModels;
using System.Windows;

namespace GitNeko.Views;

public partial class CloneDialog : Window
{
    public CloneDialog(CloneDialogViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private void OnCloseButtonClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
