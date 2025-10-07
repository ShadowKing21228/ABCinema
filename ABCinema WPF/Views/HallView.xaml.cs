using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ABCinema_WPF.ViewModels;
using MahApps.Metro.Controls.Dialogs;

namespace ABCinema_WPF.Views;

public partial class HallView : Page
{
    public HallView()
    {
        InitializeComponent();
    }
    private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is HallViewModel vm && vm.ShowHallUpdateDialogCommand.CanExecute(null))
            vm.ShowHallUpdateDialogCommand.Execute(null);
    }
}