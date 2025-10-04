using System.Windows;
using ABCinema_WPF.Services;
using ABCinema_WPF.ViewModels;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace ABCinema_WPF.Views;

public partial class RegistrationView : MetroWindow
{
    public RegistrationView()
    {
        InitializeComponent();
        DataContext = new RegistrationViewModel(DialogCoordinator.Instance, new WindowService());
    }
}