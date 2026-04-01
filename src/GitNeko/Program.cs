using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace GitNeko;

public class Program
{
    [STAThread]
    private static void Main()
    {
        var services = new ServiceCollection();

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
                MessageBox.Show(e.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            };

        TaskScheduler.UnobservedTaskException +=
            (s, e) =>
            {
                e.SetObserved();
                app.Dispatcher.Invoke(() =>
                    MessageBox.Show(e.Exception.InnerException?.Message ?? e.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error)
                );
            };

        AppDomain.CurrentDomain.UnhandledException +=
            (s, e) =>
            {
                if (e.ExceptionObject is Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };
    }
}
