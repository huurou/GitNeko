using GitNeko.Application.UseCases;
using GitNeko.Infrastructure;
using GitNeko.Services;
using GitNeko.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace GitNeko;

public class Program
{
    [STAThread]
    private static void Main()
    {
        var services = new ServiceCollection();

        // Infrastructure層（Git操作、クリップボード等）
        services.AddInfrastructure();

        // Application層（ユースケース）
        services.AddTransient<ScanRepositoriesUseCase>();
        services.AddTransient<CloneRepositoryUseCase>();

        // Presentation層（サービス、ViewModel、View）
        services.AddSingleton<IClipboardService, ClipboardService>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddTransient<MainWindowViewModel>();
        services.AddSingleton<App>();
        services.AddSingleton<MainWindow>();

        using var provider = services.BuildServiceProvider();

        var app = provider.GetRequiredService<App>();

        // グローバル例外ハンドラー登録
        HandleException(app);

        // アプリケーション実行
        app.InitializeComponent();
        var mainWindow = provider.GetRequiredService<MainWindow>();
        app.Run(mainWindow);
    }

    private static void HandleException(App app)
    {
        app.DispatcherUnhandledException +=
            (s, e) =>
            {
                e.Handled = true;
                MessageBox.Show(e.Exception.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            };

        TaskScheduler.UnobservedTaskException +=
            (s, e) =>
            {
                e.SetObserved();
                app.Dispatcher.Invoke(() =>
                    MessageBox.Show(e.Exception.InnerException?.Message ?? e.Exception.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error)
                );
            };

        AppDomain.CurrentDomain.UnhandledException +=
            (s, e) =>
            {
                if (e.ExceptionObject is Exception ex)
                {
                    MessageBox.Show(ex.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };
    }
}
