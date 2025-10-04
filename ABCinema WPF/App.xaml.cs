using System.Configuration;
using System.Data;
using System.Text;
using System.Windows;

namespace ABCinema_WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        Console.WriteLine($"{DateTime.Now:HH:mm:ss} [INFO] WPF приложение запущено");
        
        // Ваша логика запуска
        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        Console.WriteLine($"{DateTime.Now:HH:mm:ss} [INFO] WPF приложение завершено");
        base.OnExit(e);
    }
}
