using System.Windows;

namespace ABCinema_WPF.Services;

public interface INavigationService
{
    void Navigate(FrameworkElement element);
}