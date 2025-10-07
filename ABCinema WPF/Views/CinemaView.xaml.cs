using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ABCinema_WPF.ViewModels;
using MahApps.Metro.Controls.Dialogs;

namespace ABCinema_WPF.Views;

public partial class CinemaView : Page
{
    public CinemaView()
    {
        InitializeComponent();
    }

    private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is CinemaViewModel vm && vm.ShowUpdateCinemaDialogCommand.CanExecute(null))
            vm.ShowUpdateCinemaDialogCommand.Execute(null);
    }
}