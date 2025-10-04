using System.Windows;

namespace ABCinema_WPF.Services;

public interface IWindowService
{
    void Show<T>() where T : Window, new();
    void ShowDialog<T>() where T : Window, new();
    void CloseActive();
}
