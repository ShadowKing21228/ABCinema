using System.Windows.Controls;
using System.Windows.Input;
using ABCinema_WPF.ViewModels;

namespace ABCinema_WPF.Views;

public partial class GenreView : Page
{
    public GenreView()
    {
        InitializeComponent();
    }
    
    private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is GenreViewModel vm && vm.EditGenreCommand.CanExecute(null))
            vm.EditGenreCommand.Execute(null);
    }

}