using GitNeko.ViewModels;
using System.Windows;

namespace GitNeko;

public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
