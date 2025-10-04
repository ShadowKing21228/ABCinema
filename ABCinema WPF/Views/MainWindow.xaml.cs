using System.Windows;
using ABCinema_WPF.ViewModels;
using MahApps.Metro.Controls;

namespace ABCinema_WPF.Views;

public partial class MainWindow : MetroWindow
{
    public MainWindow()
    {
        InitializeComponent();
        WindowState = WindowState.Maximized;
        ResizeMode = ResizeMode.NoResize;
        DataContext = new MainWindowViewModel();
    }
    
    private void MainMenu_OnItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs args)
    {
        if (args.InvokedItem is HamburgerMenuGlyphItem item && item.Tag is Type pageType)
        {
            MainFrame.Navigate(Activator.CreateInstance(pageType));
        }
    }

}