using System.Windows;
using ABCinema_WPF.Controls;
using ABCinema_WPF.Services;
using ABCinema_WPF.ViewModels;
using MahApps.Metro.Controls;

namespace ABCinema_WPF.Views;

public partial class MainWindow : MetroWindow, INavigationService
{
    public MainWindow()
    {
        InitializeComponent();
        WindowState = WindowState.Maximized;
        Loaded += OnLoaded;
    }
    
    private void MainMenu_OnItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs args)
    {
        if (args.InvokedItem is HamburgerMenuGlyphItemDc item)
            Navigate(item);
    }

    private async void OnLoaded(object sender, RoutedEventArgs e) 
        => DataContext = await MainWindowViewModel.CreateAsync(this);
    
    
    public void Navigate(FrameworkElement view) 
        => MainFrame.Content = view;
    
    
    private void Navigate(HamburgerMenuGlyphItemDc item)
    {
        if (item.Tag is FrameworkElement view)
            MainFrame.Content = view;
    }
    
}