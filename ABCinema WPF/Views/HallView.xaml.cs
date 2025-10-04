using System.Windows;
using System.Windows.Controls;
using ABCinema_WPF.ViewModels;
using MahApps.Metro.Controls.Dialogs;

namespace ABCinema_WPF.Views;

public partial class HallView : Page
{
    public HallView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
        => DataContext = await HallViewModel.HallViewModelFactory(DialogCoordinator.Instance);
}