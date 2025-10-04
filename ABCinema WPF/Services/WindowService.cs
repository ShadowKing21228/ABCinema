using System.Windows;

namespace ABCinema_WPF.Services;

public class WindowService : IWindowService
{
    public void Show<T>() where T : Window, new()
    {
        new T().Show();
    }

    public void ShowDialog<T>() where T : Window, new()
    {
        new T().ShowDialog();
    }

    public void CloseActive()
    {
        Application.Current.Windows
            .OfType<Window>()
            .FirstOrDefault(w => w.IsActive)?.Close();
    }
}
