using System.Windows;
using System.Windows.Controls;
using ABCinema_WPF.ViewModels;
using MahApps.Metro.Controls.Dialogs;

namespace ABCinema_WPF.Views;

public partial class CinemaView : Page
{
    public CinemaView()
    {
        InitializeComponent();
        Loaded += CinemaViewLoaded;
    }
    private async void CinemaViewLoaded(object sender, RoutedEventArgs e)
    {
        DataContext = await CinemaViewModel.CreateAsync(DialogCoordinator.Instance);
    }
}